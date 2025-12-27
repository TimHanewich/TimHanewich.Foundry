using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ToolCallOutput : Exchange
    {
        public string CallId {get; set;}
        public string Output {get; set;}

        public ToolCallOutput()
        {
            CallId = string.Empty;
            Output = string.Empty;
        }

        public ToolCallOutput(string call_id, string output)
        {
            CallId = call_id;
            Output = output;
        }

        public override JObject ToJSON()
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("type", "function_call_output");
            ToReturn.Add("call_id", CallId);
            ToReturn.Add("output", Output);
            return ToReturn;
        }

    }
}