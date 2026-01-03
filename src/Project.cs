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
    public class Project //represents a Foundry project
    {
        public string Endpoint {get; set;}          //i.e. https://myfoundry.services.ai.azure.com
        public string? ApiKey {get; set;}           //API key directly provided by the Foundry portal
        public string? AccessToken {get; set;}      //Access token obtained using Entra ID Authentication (Service Principal-based)

        public Project()
        {
            Endpoint = "";
            ApiKey = null;
            AccessToken = null;
        }

        public Project(string endpoint)
        {
            Endpoint = endpoint;
            ApiKey = null;
            AccessToken = null;
        }

        public Project(string endpoint, string? api_key = null, string? access_token = null)
        {
            Endpoint = endpoint;
            ApiKey = api_key;
            AccessToken = access_token;
        }   
    }
}