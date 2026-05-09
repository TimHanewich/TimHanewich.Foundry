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
        public string Endpoint {get;}               //i.e. https://myfoundry.services.ai.azure.com
        public string? ApiKey {get; set;}           //API key directly provided by the Foundry portal
        public string? AccessToken {get; set;}      //Access token obtained using Entra ID Authentication (Service Principal-based)

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

        public HttpRequestMessage PrepareRequestMessage()
        {
            //Prepare HTTP Request
            HttpRequestMessage req = new HttpRequestMessage();

            //Plug in authentication
            if (ApiKey != null) //default to using the API key (i.e. if they provide both for some reason, use API key)
            {
                req.Headers.Add("api-key", ApiKey);
            }
            else if (AccessToken != null)
            {
                req.Headers.Add("Authorization", "Bearer " + AccessToken);
            }
            else // If neither are provided
            {
                throw new Exception("Aborting call to Foundry service: neither an API key nor Access Token was provided to access Foundry project at '" + fr.Endpoint + "'. One of these is required to authenticate with the Foundry service!");
            }

            return req;
        }
    }
}