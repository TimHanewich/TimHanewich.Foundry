using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Exchange //represents an exchange of data between the AI and the party using it (could be conversational message, a tool call, etc.)
    {
        public Exchange()
        {
            
        }

        //Basic overridable function
        public virtual JObject ToJSON()
        {
            return new JObject();
        }
    }
}