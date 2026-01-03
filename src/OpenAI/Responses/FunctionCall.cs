using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class FunctionCall : Exchange
    {
        public string FunctionName {get; set;}      //parameter "name"
        public JObject Arguments {get; set;}    //parameter "arguments"
        public string CallId {get; set;}        //parameter "call_id"

        public FunctionCall()
        {
            FunctionName = "";
            Arguments = new JObject();
            CallId = "";
        }

        public static FunctionCall Parse(JObject function_call)
        {
            FunctionCall ToReturn = new FunctionCall();

            //Get function name
            JProperty? name = function_call.Property("name");
            if (name != null)
            {
                ToReturn.FunctionName = name.Value.ToString();
            }

            //Get arguments
            JProperty? arguments = function_call.Property("arguments");
            if (arguments != null)
            {
                string arguments_json = arguments.Value.ToString();
                ToReturn.Arguments = JObject.Parse(arguments_json);
            }

            //get function call ID
            JProperty? id = function_call.Property("call_id");
            if (id != null)
            {
                ToReturn.CallId = id.Value.ToString();
            }

            return ToReturn;
        }


    }
}