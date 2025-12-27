using System;
using System.Runtime.Intrinsics.Arm;
using TimHanewich.Foundry.OpenAI.Responses;
using Newtonsoft.Json;

namespace testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ResponseRequest rr = new ResponseRequest();
            rr.Model = "gpt-5-mini-testing";
            rr.Messages.Add(new Message(Role.user, "Hello my friend."));

            Tool CheckWeather = new Tool();
            CheckWeather.Name = "CheckWeather";
            CheckWeather.Description = "Check the weather for any zip code.";
            CheckWeather.Parameters.Add(new ToolInputParameter("zip_code", "Zip code of the area you want to check the weather for"));
            rr.Tools.Add(CheckWeather);

            Console.WriteLine(rr.ToJSON().ToString());
            Console.ReadLine();

            Deployment d = new Deployment();
            d.Endpoint = "https://ai-testaistudio030597089470.openai.azure.com/openai/responses?api-version=2025-04-01-preview";
            d.ApiKey = "Ax5hHeaVUqSipUxMkrQoecnHRsqPGl289HuaEMshHTemFhQpZtSoJQQJ99BGACHYHv6XJ3w3AAAAACOGdUF4";

            Response r = d.CreateResponseAsync(rr).Result;
            Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

        }
    }
}