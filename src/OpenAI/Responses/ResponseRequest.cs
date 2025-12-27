using System;
using System.Collections.Generic;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ResponseRequest
    {
        public string Model {get; set;}
        public List<Tool> Tools {get; set;}
        public List<Exchange> Exchanges {get; set;}

        public ResponseRequest()
        {
            Model = string.Empty;
            Tools = new List<Tool>();
            Exchanges = new List<Exchange>();
        }
    }
}