using System;

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
    }
}