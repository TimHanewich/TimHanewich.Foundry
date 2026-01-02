using System;

namespace TimHanewich.Foundry
{
    public class TokenCredential
    {
        public DateTime Expires {get; set;}
        public string AccessToken {get; set;}

        public TokenCredential()
        {
            AccessToken = string.Empty;   
        }

        public TokenCredential(int expires_in, string access_token)
        {
            Expires = DateTime.Now.AddSeconds(expires_in);
            AccessToken = access_token;
        }

        public bool IsExpired()
        {
            return DateTime.Now >= Expires;
        }
    }
}