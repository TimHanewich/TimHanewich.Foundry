using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Encodings;
using System.Text;
using System.Net.Http;
using System.Net;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Deployment
    {
        public string Endpoint {get; set;}
        public string ApiKey {get; set;}

        public Deployment()
        {
            Endpoint = "";
            ApiKey = "";
        }

        public Deployment(string endpoint, string api_key)
        {
            Endpoint = endpoint;
            ApiKey = api_key;
        }

        public async Task<Response> CreateResponseAsync(ResponseRequest request)
        {

            //Prepare HTTP Request
            HttpRequestMessage req = new HttpRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(Endpoint);
            req.Headers.Add("api-key", ApiKey);

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

                //Is it a function call
                if (type.Value.ToString() == "function_call")
                {
                    ToolCall tc = ToolCall.Parse(jo);
                    outputs.Add(tc);
                }
            }
            ToReturn.Outputs = outputs.ToArray();

            return ToReturn;

        }

        // public async Task<InferenceResponse> InvokeInferenceAsync(Message[] inputs, Tool[] tools, bool json_mode)
        // {
        //     return new InferenceResponse();
        //     HttpRequestMessage req = new HttpRequestMessage();
        //     req.Method = HttpMethod.Post;
        //     req.RequestUri = new Uri(Endpoint);
        //     req.Headers.Add("api-key", ApiKey);

        //     JObject body = new JObject();

        //     //Add inputs
        //     JArray jinputs = new JArray();
        //     foreach (Message msg in inputs)
        //     {
        //         //Create and add
        //         JObject ThisMsg = new JObject();
        //         jinputs.Add(ThisMsg);

        //         //Add role
        //         ThisMsg.Add("role", msg.Role.ToString());

        //         //add content
        //         ThisMsg.Add("content", msg.Content);

        //         //Add tool calls
        //         if (msg.ToolCalls.Length > 0)
        //         {
        //             JArray tool_calls = new JArray();
        //             ThisMsg.Add("tool_calls", tool_calls);
        //             foreach (ToolCall tc in msg.ToolCalls)
        //             {
        //                 JObject tool_call = new JObject();

        //                 //Add type and ID
        //                 tool_call.Add("type", "function");
        //                 tool_call.Add("id", tc.ID);

        //                 //function
        //                 JObject function = new JObject();
        //                 tool_call.Add("function", function);
        //                 function.Add("name", tc.ToolName);
        //                 function.Add("arguments", tc.Arguments.ToString()); //add arguments as JSON-encoded string (this is how it is supposed to be, per API specification)

        //                 tool_calls.Add(tool_call);
        //             }
        //         }

        //         //Add tool call ID?
        //         if (msg.ToolCallID != null)
        //         {
        //             ThisMsg.Add("tool_call_id", msg.ToolCallID);
        //         }
        //     }
        //     body.Add("inputs", jinputs);

        //     //Add tools
        //     if (tools.Length > 0)
        //     {
        //         JArray jtools = new JArray();
        //         foreach (Tool tool in tools)
        //         {
        //             jtools.Add(tool.ToJSON());
        //         }
        //         body.Add("tools", jtools);
        //     }

        //     //Json mode?
        //     if (json_mode)
        //     {
        //         JObject response_format = new JObject();
        //         response_format.Add("type", "json_object");
        //         body.Add("response_format", response_format);
        //     }

        //     //Make API call
        //     req.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json"); //add body to request body
        //     HttpClient hc = new HttpClient();
        //     hc.Timeout = new TimeSpan(24, 0, 0);
        //     HttpResponseMessage resp = await hc.SendAsync(req);
        //     string content = await resp.Content.ReadAsStringAsync();
        //     if (resp.StatusCode != HttpStatusCode.OK)
        //     {
        //         throw new Exception("Call to model failed with code '" + resp.StatusCode.ToString() + "'. Msg: " + content);
        //     }
        //     JObject contentjo = JObject.Parse(content);

        //     //To return
        //     InferenceResponse ToReturn = new InferenceResponse();

        //     //Get prompt tokens
        //     JToken? input_tokens = contentjo.SelectToken("usage.input_tokens");
        //     if (input_tokens != null)
        //     {
        //         ToReturn.PromptTokensConsumed = Convert.ToInt32(input_tokens.ToString());
        //     }

        //     //Get completion tokens
        //     JToken? output_tokens = contentjo.SelectToken("usage.output_tokens");
        //     if (output_tokens != null)
        //     {
        //         ToReturn.CompletionTokensConsumed = Convert.ToInt32(output_tokens.ToString());
        //     }
            
        //     //Strip out message portion
        //     JToken? message = contentjo.SelectToken("choices[0].message");
        //     if (message == null)
        //     {
        //         throw new Exception("Property 'message' not in model's response. Full content of response: " + contentjo.ToString());
        //     }
        //     JObject ResponseMessage = (JObject)message;
        //     ToReturn.Message = Message.Parse(ResponseMessage);

        //     return ToReturn;
        // }
    
    
    }
}