using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ToolCall : Exchange
    {
        public string ToolName {get; set;}      //parameter "name"
        public JObject Arguments {get; set;}    //parameter "arguments"
        public string CallId {get; set;}       //parameter "call_id"

        public ToolCall()
        {
            ToolName = "";
            Arguments = new JObject();
            CallId = "";
        }

        public static ToolCall Parse(JObject tool_call)
        {
            ToolCall ToReturn = new ToolCall();

            //Get tool name
            JProperty? name = tool_call.Property("name");
            if (name != null)
            {
                ToReturn.ToolName = name.Value.ToString();
            }

            //Get arguments
            JProperty? arguments = tool_call.Property("arguments");
            if (arguments != null)
            {
                string arguments_json = arguments.Value.ToString();
                ToReturn.Arguments = JObject.Parse(arguments_json);
            }

            //get tool call ID
            JProperty? id = tool_call.Property("call_id");
            if (id != null)
            {
                ToReturn.CallId = id.Value.ToString();
            }

            return ToReturn;
        }


    }
}