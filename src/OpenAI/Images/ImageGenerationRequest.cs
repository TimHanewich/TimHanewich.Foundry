using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//https://developers.openai.com/api/docs/guides/image-generation#generate-images

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageGenerationRequest : ImageRequest
    {
        public ImageGenerationRequest()
        {
        }

        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            ToReturn.Add("model", Model);
            ToReturn.Add("prompt", Prompt);
            ToReturn.Add("size", Width.ToString() + "x" + Height.ToString());
            ToReturn.Add("n", Count);

            //Quality
            if (Quality == ImageQuality.Auto)
            {
                ToReturn.Add("quality", "auto");
            }
            else if (Quality == ImageQuality.Low)
            {
                ToReturn.Add("quality", "low");
            }
            else if (Quality == ImageQuality.Medium)
            {
                ToReturn.Add("quality", "medium");
            }
            else if (Quality == ImageQuality.High)
            {
                ToReturn.Add("quality", "high");
            }

            return ToReturn;
        }
    }
}