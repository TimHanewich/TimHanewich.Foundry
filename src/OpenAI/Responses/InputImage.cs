using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class InputImage
    {
        public string URL {get; set;}
        public InputImageDetail Detail {get; set;}

        public InputImage()
        {
            URL = "";
            Detail = InputImageDetail.Auto;
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
    
        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();
            ToReturn.Add("type", "input_image");
            ToReturn.Add("image_url", URL);

            //Detail
            if (Detail == InputImageDetail.Low)
            {
                ToReturn.Add("detail", "low");
            }
            else if (Detail == InputImageDetail.High)
            {
                ToReturn.Add("detail", "high");
            }
            else if (Detail == InputImageDetail.Original)
            {
                ToReturn.Add("detail", "original");
            }
            else if (Detail == InputImageDetail.Auto)
            {
                ToReturn.Add("detail", "auto");
            }

            return ToReturn;
        }
    }
}