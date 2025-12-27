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
            //Define the deployment
            Deployment d = new Deployment();
            d.Endpoint = "https://ai-testaistudio020597089470.openai.azure.com/openai/responses?api-version=2025-04-01-preview";
            d.ApiKey = "Ax5hHeaVUqSipUxMkr...";

            //Create a response request (uses the Responses API)
            ResponseRequest rr = new ResponseRequest();
            rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
            rr.Inputs.Add(new Message(Role.user, "Parse out the first and last name and provide it to me in JSON like this format, as an example: {'first': 'Ron', 'last': 'Weasley'}.\n\n'Hi, my name is Harold Gargon."));
            rr.RequestedFormat = ResponseFormat.JsonObject; //specify you want a JSON object output ('JSON mode')

            //Call to API service
            Response r = d.CreateResponseAsync(rr).Result;

            //Print response info
            Console.WriteLine("Response ID: " + r.Id);
            Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
            Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
            foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
            {
                if (exchange is Message msg) //if this output is a Message
                {
                   Console.WriteLine("Response: " + msg.Text);
                }
            }


        }
    }
}