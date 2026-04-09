using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class JsonSchema
    {
        public string Name {get; set;}
        public bool Strict {get; set;}
        public JObject? Schema {get; set;}

        public JsonSchema()
        {
            Name = string.Empty;
            Strict = true;
            Schema = null;
        }

        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //type
            ToReturn.Add("type", "json_schema");

            //strict
            ToReturn.Add("strict", true);

            //schema
            ToReturn.Add("schema", Schema);

            return ToReturn;
        }
    }
}