using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageEditRequest : ImageRequest
    {
        public List<AttachedImage> AttachedImages {get; set;}

        public ImageEditRequest()
        {
            AttachedImages = new List<AttachedImage>();
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