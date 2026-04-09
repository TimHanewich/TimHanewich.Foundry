using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class StructuredOutputSchema
    {
        public string Name {get; set;}
        public bool Strict {get; set;}
        public JObject Schema {get; set;}

        public StructuredOutputSchema()
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

            //schema
            ToReturn.Add("schema", Schema);

            return ToReturn;
        }
    }
}