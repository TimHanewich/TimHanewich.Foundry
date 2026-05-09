using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//https://developers.openai.com/api/docs/guides/image-generation#generate-images

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageGenerationRequest
    {
        public string Model {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public string Prompt {get; set;}
        public int Count {get; set;}

        public ImageGenerationRequest()
        {
            Model = string.Empty;
            Prompt = string.Empty;
            Count = 1; //default
        }

        public void SetCommonSize(ImageSize size)
        {
            //Set width + height accordingly
        }

        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            ToReturn.Add("model", Model);
            ToReturn.Add("prompt", Prompt);
            ToReturn.Add("size", Width.ToString() + "x" + Height.ToString());
            ToReturn.Add("n", Count);

            return ToReturn;
        }
    }
}