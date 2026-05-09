using System;
using Newtonsoft.Json.Linq;

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

        public static ImageGeneration Parse(JObject payload)
        {
            //Create
            ImageGeneration ToReturn = new ImageGeneration();

            //Get each image
            List<Image> imgs = new List<Image>();
            JProperty? prop_data = payload.Property("data");
            if (prop_data != null)
            {
                if (prop_data.Value.Type == JTokenType.Array)
                {
                    JArray dataJArray = (JArray)prop_data.Value;
                    foreach (JObject jo in dataJArray)
                    {
                        JProperty? prop_b64_json = jo.Property("b64_json");
                        if (prop_b64_json != null)
                        {
                            string b64 = prop_b64_json.Value.ToString();
                            imgs.Add(new Image(b64));
                        }
                    }
                }
            }
            ToReturn.Images = imgs.ToArray();

            //Token usage: Input Tokens
            JToken? InputTokens = payload.SelectToken("usage.input_tokens");        
            if (InputTokens != null)
            {
                if (InputTokens.Type == JTokenType.Integer)
                {
                    ToReturn.InputTokens = Convert.ToInt32(InputTokens.ToString());
                }
            }    

            //Token usage: Output Tokens
            JToken? OutputTokens = payload.SelectToken("usage.output_tokens");
            if (OutputTokens != null)
            {
                if (OutputTokens.Type == JTokenType.Integer)
                {
                    ToReturn.OutputTokens = Convert.ToInt32(OutputTokens.ToString());
                }
            }

            //Return
            return ToReturn;
        }
    }
}