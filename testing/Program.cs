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
            FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
            fr.ApiKey = "6ElIJZ2jsMM...";

            ResponseRequest rr = new ResponseRequest();
            rr.Model = "gpt-5-nano";
            rr.Inputs.Add(new Message(Role.user, "Why is the sky blue?"));

            Response r = fr.CreateResponseAsync(rr).Result;
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
        }
    }
}