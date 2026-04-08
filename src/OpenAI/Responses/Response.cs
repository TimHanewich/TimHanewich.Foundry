using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    //A collection of data that comes back from an API call to an LLM service
    public class Response
    {
        public string Id {get; set;} // id of the response
        public DateTimeOffset CreatedAt {get; set;}
        public ResponseStatus Status {get; set;}
        public string Model {get; set;} //What model fulfilled it
        public bool Blocked {get; set;} //blocked for any reason (i.e. prompt or completion)
        public Exchange[] Outputs {get; set;}
        public int InputTokensConsumed {get; set;}
        public int OutputTokensConsumed {get; set;}

        public Response()
        {
            Id = string.Empty;
            Outputs = new Exchange[]{};
            Blocked = false;
            Model = string.Empty;
        }
    }
}