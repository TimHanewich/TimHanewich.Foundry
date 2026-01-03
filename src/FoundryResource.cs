using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Encodings;
using System.Text;
using System.Net.Http;
using System.Net;

namespace TimHanewich.Foundry
{
    public class FoundryResource //represents a Foundry Resource
    {
        public string Endpoint {get; set;}          //i.e. https://myfoundry.services.ai.azure.com
        public string? ApiKey {get; set;}           //API key directly provided by the Foundry portal
        public string? AccessToken {get; set;}      //Access token obtained using Entra ID Authentication (Service Principal-based)

        public FoundryResource()
        {
            Endpoint = "";
            ApiKey = null;
            AccessToken = null;
        }

        public FoundryResource(string endpoint)
        {
            Endpoint = StripUrlToBase(endpoint);
            ApiKey = null;
            AccessToken = null;
        }

        public FoundryResource(string endpoint, string? api_key = null, string? access_token = null)
        {
            Endpoint = StripUrlToBase(endpoint);
            ApiKey = api_key;
            AccessToken = access_token;
        }

        private string StripUrlToBase(string original) //strips down to just the ".com" portion, nothing after
        {
            Uri u = new Uri(original);
            return u.GetLeftPart(UriPartial.Authority);
        }
    }
}