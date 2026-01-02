using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry
{
    public class EntraAuthenticationHandler
    {
        public string TenantID {get; set;}         //Tenant (Directory) ID
        public string ClientID {get; set;}         //Application (Client) ID
        public string ClientSecret {get; set;}     //Client Secret (value)

        public EntraAuthenticationHandler()
        {
            TenantID = string.Empty;
            ClientID = string.Empty;
            ClientSecret = string.Empty;
        }

        public async Task<TokenCredential> AuthenticateAsync()
        {
            //Ensure all required properties are included
            if (TenantID == string.Empty)
            {
                throw new Exception("Aborting application authentication: 'TenantID' property left blank!");
            }
            if (ClientID == string.Empty)
            {
                throw new Exception("Aborting application authentication: 'ClientID' property left blank!");
            }
            if (ClientSecret == string.Empty)
            {
                throw new Exception("Aborting application authentication: 'ClientSecret' property left blank!");
            }

            HttpRequestMessage req = new HttpRequestMessage();

            //Set up URL
            string url = "https://login.microsoftonline.com/" + TenantID + "/oauth2/v2.0/token";
            req.RequestUri = new Uri(url);

            //Set up properties we will provide as Form URL encoded content
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("grant_type", "client_credentials");
            body.Add("scope", "https://cognitiveservices.azure.com/.default"); //want to use Foundry services
            body.Add("client_id", ClientID);
            body.Add("client_secret", ClientSecret);
            req.Content = new FormUrlEncodedContent(body);

            //Make the call!
            HttpClient hc = new HttpClient();
            HttpResponseMessage resp;
            try
            {
                resp = await hc.SendAsync(req);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to call Foundry service to authenticate: " + ex.Message);
            }
            string content = await resp.Content.ReadAsStringAsync();

            //Error occured?
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to authenticate returned '" + resp.StatusCode.ToString() + "': " + content);
            }

            //Parse the resulting body
            JObject payload = JObject.Parse(content);

            //Get the expires in property (int)
            JProperty? prop_expiresin = payload.Property("expires_in");
            if (prop_expiresin == null)
            {
                throw new Exception("Unable to find property 'expires_in' in returned authentication payload!");
            }
            int expires_in = Convert.ToInt32(prop_expiresin.Value.ToString());

            //Get the "access_token" property
            JProperty? prop_access_token = payload.Property("access_token");
            if (prop_access_token == null)
            {
                throw new Exception("Unable to find property 'access_token' in returned authentiation payload!");
            }
            string access_token = prop_access_token.Value.ToString();

            //return the payload
            TokenCredential ToReturn = new TokenCredential(expires_in, access_token);
            return ToReturn;
        }
    }
}