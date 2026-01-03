using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Tool
    {
        public virtual JObject ToJSON()
        {
            return new JObject();
        }
    }
}