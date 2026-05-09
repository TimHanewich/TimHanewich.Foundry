using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageEditRequest
    {
        public string Model {get; set;}
        public string Prompt {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public int Count {get; set;}
        public ImageQuality Quality {get; set;}
        public List<AttachedImage> AttachedImages {get; set;}

        public ImageEditRequest()
        {
            Model = string.Empty;
            Prompt = string.Empty;
            AttachedImages = new List<AttachedImage>();
            Count = 1;
        }

        //Prepare
        public MultipartFormDataContent Prepare()
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(Model), "model");
            content.Add(new StringContent(Prompt), "prompt");
            content.Add(new StringContent(Width.ToString() + "x" + Height.ToString()), "size");
            content.Add(new StringContent(Count.ToString()), "n");
            
            //Add quality
            if (Quality == ImageQuality.Auto)
            {
                content.Add(new StringContent("auto"), "quality");
            }
            else if (Quality == ImageQuality.Low)
            {
                content.Add(new StringContent("low"), "quality");
            }
            else if (Quality == ImageQuality.Medium)
            {
                content.Add(new StringContent("medium"), "quality");
            }
            else if (Quality == ImageQuality.High)
            {
                content.Add(new StringContent("high"), "quality");
            }

            //Add each file
            foreach (AttachedImage attimg in AttachedImages)
            {
                content.Add(new ByteArrayContent(attimg.Bytes), "image[]", attimg.FileName);
            }

            //Return 
            return content;
        }
    }
}