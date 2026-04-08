using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class InputImage
    {
        public string URL {get; set;}

        public InputImage()
        {
            URL = "";
        }

        public static InputImage FromFile(string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                throw new Exception("File at '" + path + "' does not exist.");
            }
            
            //Check extension
            string extension = Path.GetExtension(path);
            string[] allowed = new string[]{".png", ".jpg", ".jpeg", ".gif", ".webp"};
            if (allowed.Contains(extension.ToLower()) == false)
            {
                throw new Exception("File '" + extension + "' is not a format that works.");
            }

            //Get base64
            byte[] bytes = File.ReadAllBytes(path);
            string base64 = Convert.ToBase64String(bytes);
            string full_url = "data:image/" + extension.Replace(".", "") + ";base64," + base64;

            //Plug it in
            InputImage ToReturn = new InputImage();
            ToReturn.URL = full_url;
            return ToReturn;
        }

        public static InputImage FromURL(string url)
        {
            InputImage ToReturn = new InputImage();
            ToReturn.URL = url;
            return ToReturn;
        }
    }
}