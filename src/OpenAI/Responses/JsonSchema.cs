using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class JsonSchema
    {
        public string Name {get; set;}
        public bool Strict {get; set;}
        public JObject Schema {get; set;}

        public JsonSchema()
        {
            Name = string.Empty;
            Strict = true;
            Schema = new JObject();
        }

        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //type
            ToReturn.Add("type", "json_schema");

            //name
            ToReturn.Add("name", Name);

            //strict
            ToReturn.Add("strict", true);

            //Ensure schema has "additionalProperties" and it is set to false (prob doesnt have it)
            JProperty? prop_additionalProperties = Schema.Property("additionalProperties");
            if (prop_additionalProperties == null)
            {
                Schema.Add("additionalProperties", false);
            }
            else
            {
                Schema["additionalProperties"] = false;
            }

            //schema
            ToReturn.Add("schema", Schema);

            return ToReturn;
        }
    }
}