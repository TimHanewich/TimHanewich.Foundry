using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class WebSearchCallQuery : Exchange
    {
        public string Query {get; set;}
        
        public WebSearchCallQuery()
        {
            Query = string.Empty;
        }

        public static WebSearchCallQuery Parse(JObject ws)
        {
            WebSearchCallQuery ToReturn = new WebSearchCallQuery();

            JToken? query = ws.SelectToken("action.query");
            if (query != null)
            {
                ToReturn.Query = query.ToString();
            }

            return ToReturn;
        }
    }
}