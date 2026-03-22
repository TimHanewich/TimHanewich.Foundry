using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public static class FoundryResourceExtensions
    {
        private static HttpRequestMessage PrepareRequestMessage(this FoundryResource fr)
        {
            //Prepare HTTP Request
            HttpRequestMessage req = new HttpRequestMessage();

            //Plug in authentication
            if (fr.ApiKey != null) //default to using the API key (i.e. if they provide both for some reason, use API key)
            {
                req.Headers.Add("api-key", fr.ApiKey);
            }
            else if (fr.AccessToken != null)
            {
                req.Headers.Add("Authorization", "Bearer " + fr.AccessToken);
            }
            else // If neither are provided
            {
                throw new Exception("Aborting call to Foundry service: neither an API key nor Access Token was provided to access Foundry project at '" + fr.Endpoint + "'. One of these is required to authenticate with the Foundry service!");
            }

            return req;
        }

        private static Response ParseResponseFromPayload(JObject payload)
        {
            //To return
            Response ToReturn = new Response();

            //Get the response id
            JProperty? prop_id = payload.Property("id");
            if (prop_id != null)
            {
                ToReturn.Id = prop_id.Value.ToString();
            }

            //Get created_at (UNIX timestamp)
            JProperty? prop_created_at = payload.Property("created_at");
            if (prop_created_at != null)
            {
                if (prop_created_at.Value.Type == JTokenType.Integer)
                {
                    int created_at = Convert.ToInt32(prop_created_at.Value.ToString());
                    ToReturn.CreatedAt = DateTimeOffset.FromUnixTimeSeconds(created_at);
                }
            }

            //Get the status
            JProperty? prop_status = payload.Property("status");
            if (prop_status != null)
            {
                string status = prop_status.Value.ToString();
                if (status == "queued")
                {
                    ToReturn.Status = ResponseStatus.Queued;
                }
                else if (status == "in_progress")
                {
                    ToReturn.Status = ResponseStatus.InProgress;
                }
                else if (status == "completed")
                {
                    ToReturn.Status = ResponseStatus.Completed;
                }
                else if (status == "failed")
                {
                    ToReturn.Status = ResponseStatus.Failed;
                }
                else if (status == "cancelled")
                {
                    ToReturn.Status = ResponseStatus.Cancelled;
                }
                else if (status == "incomplete")
                {
                    ToReturn.Status = ResponseStatus.Incomplete;
                }
                else
                {
                    ToReturn.Status = ResponseStatus.Unknown;
                }
            }

            //Get the blocked status
            JProperty? prop_content_filters = payload.Property("content_filters");
            if (prop_content_filters != null)
            {
                if (prop_content_filters.Value.Type == JTokenType.Array)
                {
                    JArray cfs = (JArray)prop_content_filters.Value;
                    foreach (JObject jo in cfs)
                    {
                        JProperty? prop_blocked = jo.Property("blocked");
                        if (prop_blocked != null)
                        {
                            try //place in try bracket just in case that "blocked" value does not convert to boolean.
                            {
                                bool blocked = Convert.ToBoolean(prop_blocked.Value.ToString());
                                if (ToReturn.Blocked == false && blocked == true)
                                {
                                    ToReturn.Blocked = blocked;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("That failed");
                            }
                        }
                    }
                }
            }

            //Get input tokens
            JToken? input_tokens = payload.SelectToken("usage.input_tokens");
            if (input_tokens != null)
            {
                ToReturn.InputTokensConsumed = Convert.ToInt32(input_tokens.ToString());
            }

            //Get output tokens
            JToken? output_tokens = payload.SelectToken("usage.output_tokens");
            if (output_tokens != null)
            {
                ToReturn.OutputTokensConsumed = Convert.ToInt32(output_tokens.ToString());
            }

            //Get outputs
            List<Exchange> outputs = new List<Exchange>();
            JToken? output_selection = payload.SelectToken("output");
            if (output_selection == null)
            {
                throw new Exception("Property 'message' not in model's response. Full content of response: " + payload.ToString());
            }
            JArray outputs_ja = (JArray)output_selection;
            foreach (JObject jo in outputs_ja)
            {
                //Get type
                JProperty? type = jo.Property("type");
                if (type == null)
                {
                    continue; //skip ahead to the next item in the array (skip this one)
                }

                //Parse based on type
                if (type.Value.ToString() == "function_call") // function call?
                {
                    FunctionCall tc = FunctionCall.Parse(jo);
                    outputs.Add(tc);
                }
                else if (type.Value.ToString() == "message") //message?
                {
                    Message msg = Message.Parse(jo);
                    outputs.Add(msg);
                }
                else if (type.Value.ToString() == "web_search_call") //web search call: query OR open page
                {
                    JToken? ActionType = jo.SelectToken("action.type");
                    if (ActionType != null)
                    {
                        if (ActionType.ToString() == "search") //web search, with a query
                        {
                            outputs.Add(WebSearchCallQuery.Parse(jo));
                        }
                        else if (ActionType.ToString() == "open_page")
                        {
                            outputs.Add(new WebSearchCallOpenPage()); //it does not specify which page was opened
                        }
                    }
                }
                else if (type.Value.ToString() == "code_interpreter_call") // code interpreter used
                {
                    outputs.Add(CodeInterpreterCall.Parse(jo));
                }
            }
            ToReturn.Outputs = outputs.ToArray();

            return ToReturn;
        }

        //Creates a new response by submitted a new response request
        public static async Task<Response> CreateResponseAsync(this FoundryResource fr, ResponseRequest request)
        {
            //Prepare URL endpoint we will call to
            UriBuilder builder = new UriBuilder(fr.Endpoint);
            builder.Path = "/openai/responses";
            builder.Query = "api-version=2025-04-01-preview";
            string endpoint = builder.Uri.ToString();

            //Prepare HTTP Request
            HttpRequestMessage req = fr.PrepareRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(endpoint);

            //Add body
            req.Content = new StringContent(request.ToJSON().ToString(), Encoding.UTF8, "application/json");

            //Make API call
            HttpClient hc = new HttpClient();
            hc.Timeout = new TimeSpan(24, 0, 0);
            HttpResponseMessage resp = await hc.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to model failed with code '" + resp.StatusCode.ToString() + "'. Msg: " + content);
            }
            JObject payload = JObject.Parse(content);

            //Parse
            Response ToReturn = ParseResponseFromPayload(payload);

            return ToReturn;
        }
    
        //Retrieves an existing response by ID
        public static async Task<Response> RetrieveResponseAsync(this FoundryResource fr, string id)
        {
            //Prepare URL endpoint we will call to
            UriBuilder builder = new UriBuilder(fr.Endpoint);
            builder.Path = "/openai/responses/" + id;
            builder.Query = "api-version=2025-04-01-preview";
            string endpoint = builder.Uri.ToString();

            //Prepare HTTP Request
            HttpRequestMessage req = fr.PrepareRequestMessage();
            req.Method = HttpMethod.Get;
            req.RequestUri = new Uri(endpoint);

            //Make API call
            HttpClient hc = new HttpClient();
            hc.Timeout = new TimeSpan(24, 0, 0);
            HttpResponseMessage resp = await hc.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to model failed with code '" + resp.StatusCode.ToString() + "'. Msg: " + content);
            }
            JObject payload = JObject.Parse(content);

            //Parse
            Response ToReturn = ParseResponseFromPayload(payload);

            return ToReturn;
        }
    }
}