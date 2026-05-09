using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class ImageGeneration
    {
        public int InputTokens {get; set;}
        public int OutputTokens {get; set;}
        public Image[] Images {get; set;}

        public ImageGeneration()
        {
            Images = new Image[]{};
        }
    }
}