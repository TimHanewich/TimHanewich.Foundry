using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public static class DeploymentExtensions
    {
        public static async Task<Response> CreateResponseAsync(this Deployment d, ResponseRequest request)
        {

            //Prepare HTTP Request
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(d.Endpoint);

            //Plug in authentication
            if (d.ApiKey != null) //default to using the API key (i.e. if they provide both for some reason, use API key)
            {
                req.Headers.Add("api-key", d.ApiKey);
            }
            else if (d.AccessToken != null)
            {
                req.Headers.Add("Authorization", "Bearer " + d.AccessToken);
            }
            else // If neither are provided
            {
                throw new Exception("Aborting call to Foundry service: neither an API key nor Access Token was provided to access Foundry deployment at '" + d.Endpoint + "'. One of these is required to authenticate with the Foundry service!");
            }    

            //Add body
            req.Content = new StringContent(request.ToJSON().ToString(), Encoding.UTF8, "application/json");

            //Make API call
            HttpClient hc = new HttpClient();
            hc.Timeout = new TimeSpan(24, 0, 0);
            HttpResponseMessage resp = await hc.SendAsync(req);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to model failed with code '" + resp.StatusCode.ToString() + "'. Msg: " + content);
            }
            JObject contentjo = JObject.Parse(content);

            //To return
            Response ToReturn = new Response();

            //Get the response id
            JProperty? prop_id = contentjo.Property("id");
            if (prop_id != null)
            {
                ToReturn.Id = prop_id.Value.ToString();
            }

            //Get input tokens
            JToken? input_tokens = contentjo.SelectToken("usage.input_tokens");
            if (input_tokens != null)
            {
                ToReturn.InputTokensConsumed = Convert.ToInt32(input_tokens.ToString());
            }

            //Get output tokens
            JToken? output_tokens = contentjo.SelectToken("usage.output_tokens");
            if (output_tokens != null)
            {
                ToReturn.OutputTokensConsumed = Convert.ToInt32(output_tokens.ToString());
            }

            //Get outputs
            List<Exchange> outputs = new List<Exchange>();
            JToken? output_selection = contentjo.SelectToken("output");
            if (output_selection == null)
            {
                throw new Exception("Property 'message' not in model's response. Full content of response: " + contentjo.ToString());
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
                    ToolCall tc = ToolCall.Parse(jo);
                    outputs.Add(tc);
                }
                else if (type.Value.ToString() == "message") //message?
                {
                    Message msg = Message.Parse(jo);
                    outputs.Add(msg);
                }
            }
            ToReturn.Outputs = outputs.ToArray();

            return ToReturn;

        }
    }
}