using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class CodeInterpreterTool : Tool
    {
        public override JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Type
            ToReturn.Add("type", "code_interpreter");
            
            //container
            JObject container = new JObject();
            container.Add("type", "auto");
            ToReturn.Add("container", container);

            return ToReturn;
        }
    }
}