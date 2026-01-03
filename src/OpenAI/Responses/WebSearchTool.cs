using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class WebSearchTool : Tool
    {
        public List<string> AllowedDomains {get; set;}

        public WebSearchTool()
        {
            AllowedDomains = new List<string>();
        }

        public override JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Add type
            ToReturn.Add("type", "web_search");

            //Filters?
            if (AllowedDomains.Count > 0)
            {
                JObject filters = new JObject();
                JArray allowed_domains = new JArray();
                foreach (string domain in AllowedDomains)
                {
                    allowed_domains.Add(domain);
                }
                filters.Add("allowed_domains", allowed_domains);
                ToReturn.Add("filters", filters);
            }

            return ToReturn;
        }
    }
}