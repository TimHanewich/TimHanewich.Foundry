using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageGeneration
    {
        public string ImageBase64 {get; set;}

        public ImageGeneration()
        {
            ImageBase64 = string.Empty;
        }
    }
}