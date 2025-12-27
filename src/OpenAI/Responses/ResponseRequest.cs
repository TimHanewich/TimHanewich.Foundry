using System;
using System.Collections.Generic;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ResponseRequest
    {
        public string? PreviousResponseID {get; set;}
        public List<Message> Messages {get; set;}
        public List<Tool> Tools {get; set;}
        public ReasoningEffort? ReasoningEffort {get; set;}

        public ResponseRequest()
        {
            PreviousResponseID = null;
            Tools = new List<Tool>();
            Messages = new List<Message>();
            ReasoningEffort = null;
        }
    }
}