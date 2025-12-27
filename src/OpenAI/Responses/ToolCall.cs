using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.AgentFramework.Responses
{
    public class ToolCall : Message
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
            JToken? name = tool_call.SelectToken("function.name");
            if (name != null)
            {
                ToReturn.ToolName = name.ToString();
            }

            //Get arguments
            JToken? arguments = tool_call.SelectToken("function.arguments");
            if (arguments != null)
            {
                string arguments_json = arguments.ToString();
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