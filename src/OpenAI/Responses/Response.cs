using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    //A collection of data that comes back from an API call to an LLM service
    public class Response
    {
        public string Id {get; set;} // id of the response
        public ResponseStatus Status {get; set;}
        public bool PromptBlocked {get; set;}
        public bool CompletionBlocked {get; set;}
        public Exchange[] Outputs {get; set;}
        public int InputTokensConsumed {get; set;}
        public int OutputTokensConsumed {get; set;}

        public Response()
        {
            Id = string.Empty;
            Outputs = new Exchange[]{};
            PromptBlocked = false;
            CompletionBlocked = false;
        }
    }
}