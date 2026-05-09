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
        public ImageQuality Quality {get; set;}

        public ImageGenerationRequest()
        {
            Model = string.Empty;
            Prompt = string.Empty;
            Count = 1; //default
            Quality = ImageQuality.Auto;
        }

        public void SetCommonSize(ImageSize size)
        {
            if (size == ImageSize.Size_1024x1024)
            {
                Width = 1024; Height = 1024;
            }
            else if (size == ImageSize.Size_1536x1024)
            {
                Width = 1536; Height = 1024;
            }
            else if (size == ImageSize.Size_1024x1536)
            {
                Width = 1024; Height = 1536;
            }
            else if (size == ImageSize.Size_2048x2048)
            {
                Width = 2048; Height = 2048;
            }
            else if (size == ImageSize.Size_2048x1152)
            {
                Width = 2048; Height = 1152;
            }
            else if (size == ImageSize.Size_3840x2160)
            {
                Width = 3840; Height = 2160;
            }
            else if (size == ImageSize.Size_2160x3840)
            {
                Width = 2160; Height = 3840;
            }
            else
            {
                Width = 1024; Height = 1024;
            }
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