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

        public Image(string b64)
        {
            ImageBase64 = b64;
        }

        public void Save(string path)
        {
            byte[] imgbytes = Convert.FromBase64String(ImageBase64);
            File.WriteAllBytes(path, imgbytes);
        }
    }
}