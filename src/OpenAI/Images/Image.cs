using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class Image
    {
        public string ImageBase64 {get; set;}

        public Image()
        {
            ImageBase64 = string.Empty;
        }
    }
}