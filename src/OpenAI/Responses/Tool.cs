using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Tool
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public List<ToolInputParameter> Parameters {get; set;}

        public Tool()
        {
            Name = "";
            Description = "";
            Parameters = new List<ToolInputParameter>();
        }

        public Tool(string name, string description)
        {
            Name = name;
            Description = description;
            Parameters = new List<ToolInputParameter>();
        }

        public JObject ToJSON()
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
            foreach (ToolInputParameter tip in Parameters)
            {
                JObject InfoAboutThisTIP = new JObject();
                properties.Add(tip.Name, InfoAboutThisTIP);
                InfoAboutThisTIP.Add("type", tip.ParameterType);
                InfoAboutThisTIP.Add("description", tip.Description);
            }

            //required (assuming all are required)
            JArray required = new JArray();
            parameters.Add("required", required);
            foreach (ToolInputParameter tip in Parameters)
            {
                required.Add(tip.Name);
            }

            return ToReturn;
        }
    }
}