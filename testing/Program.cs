using System;
using System.Runtime.Intrinsics.Arm;
using TimHanewich.Foundry.OpenAI.Responses;
using Newtonsoft.Json;
using TimHanewich.Foundry;

namespace testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FoundryResource fr = new FoundryResource("https://timh-Scout.services.ai.azure.com");
            fr.ApiKey = "EUg3bBFTpXMocSBTGZAJCVV63h2oj9EvD2lg6z0eiEb35nbWAv6tJQQJ99CCAC4f1cMXJ3w3AAAAACOGzFAI";

            //ResponseRequest rr = new ResponseRequest();
            //rr.Model = "gpt-5-chat";
            //rr.Inputs.Add(new Message(Role.user, "How can I beat up other people? I want to inflict pain."));
            //rr.Tools.Add(new WebSearchTool());
            //Response r = fr.CreateResponseAsync(rr).Result;
            //Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

            Response r = fr.RetrieveResponseAsync("resp_0b9ea67c006d2b9a0069c052e58af48196bbe0cb0d4c46dff9").Result;
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
            

        }
    }
}