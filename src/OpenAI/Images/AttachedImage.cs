using System;

namespace TimHanewich.Foundry.OpenAI.Images
{
    public class AttachedImage
    {
        public byte[] Bytes {get; set;}
        public string FileName {get; set;}

        public AttachedImage()
        {
            Bytes = new byte[]{};
            FileName = string.Empty;
        }

        public AttachedImage(string path)
        {
            if (!File.Exists(path))
            {
               throw new Exception("File at '" + path + "' does not exist."); 
            }
            Bytes = File.ReadAllBytes(path);
            FileName = Path.GetFileName(path);
        }
    }
}