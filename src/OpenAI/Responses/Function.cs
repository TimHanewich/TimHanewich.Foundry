using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Function : Tool
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public List<FunctionInputParameter> Parameters {get; set;}

        public Function()
        {
            Name = "";
            Description = "";
            Parameters = new List<FunctionInputParameter>();
        }

        public Function(string name, string description)
        {
            Name = name;
            Description = description;
            Parameters = new List<FunctionInputParameter>();
        }

        public override JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Type
            ToReturn.Add("type", "function");

            //Name
            ToReturn.Add("name", Name);

            //Description
            ToReturn.Add("description", Description);

            //parameters
            JObject parameters = new JObject();
            ToReturn.Add("parameters", parameters);
            parameters.Add("type", "object");

            //properties
            JObject properties = new JObject();
            parameters.Add("properties", properties);
            foreach (FunctionInputParameter tip in Parameters)
            {
                JObject InfoAboutThisTIP = new JObject();
                properties.Add(tip.Name, InfoAboutThisTIP);
                InfoAboutThisTIP.Add("type", tip.ParameterType);
                InfoAboutThisTIP.Add("description", tip.Description);
            }

            //required
            JArray required = new JArray();
            parameters.Add("required", required);
            foreach (FunctionInputParameter tip in Parameters)
            {
                if (tip.Required)
                {
                    required.Add(tip.Name);  
                }
            }

            return ToReturn;
        }
    }
}