using System;

//https://developers.openai.com/api/docs/guides/image-generation#generate-images

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageGenerationRequest
    {
        public int Width {get; set;}
        public int Height {get; set;}
        public string Prompt {get; set;}
        public int Count {get; set;}

        public ImageGenerationRequest()
        {
            Prompt = string.Empty;
        }

        public void SetCommonSize(ImageSize size)
        {
            //Set width + height accordingly
        }
    }
}