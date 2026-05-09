using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public static class FoundryResourceExtensions
    {
        public static async Task<ImageGeneration> GenerateImageAsync(this FoundryResource fr, ImageGenerationRequest request)
        {
            //Construct URL
            UriBuilder builder = new UriBuilder(fr.Endpoint);
            builder.Path = "/openai/v1/images/generations";
            string endpoint = builder.Uri.ToString();

            //Prepare HTTP request
            HttpRequestMessage req = fr.PrepareRequestMessage();
            req.Method = HttpMethod.Post;
            req.RequestUri = new Uri(endpoint);

            //Add body
            req.Content = new StringContent(request.ToJSON().ToString(), Encoding.UTF8, "application/json");

            //Make API call
            HttpClient hc = new HttpClient();
            hc.Timeout = new TimeSpan(24, 0, 0);
            HttpResponseMessage resp = await hc.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
            string content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Call to model failed with code '" + resp.StatusCode.ToString() + "'. Msg: " + content);
            }
            JObject payload = JObject.Parse(content);

            //Parse
            Console.WriteLine(payload.ToString());

            return new ImageGeneration();

        }
    }
}