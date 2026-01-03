using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class CodeInterpreterTool : Tool
    {
        public MemoryAmount? MemoryLimit {get; set;}

        public CodeInterpreterTool()
        {
            
        }

        public CodeInterpreterTool(MemoryAmount mem_limit)
        {
            MemoryLimit = mem_limit;
        }

        public override JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Type
            ToReturn.Add("type", "code_interpreter");
            
            //container
            JObject container = new JObject();
            container.Add("type", "auto");
            if (MemoryLimit.HasValue)
            {
                if (MemoryLimit.Value == MemoryAmount.gb1)
                {
                    container.Add("memory_limit", "1g");
                }
                else if (MemoryLimit.Value == MemoryAmount.gb4)
                {
                    container.Add("memory_limit", "4g");
                }
                else if (MemoryLimit.Value == MemoryAmount.gb16)
                {
                    container.Add("memory_limit", "16g");
                }
                else if (MemoryLimit.Value == MemoryAmount.gb64)
                {
                    container.Add("memory_limit", "64g");
                }
            }
            ToReturn.Add("container", container);

            return ToReturn;
        }

        public enum MemoryAmount
        {
            gb1 = 0, //1 gb
            gb4 = 1, //4 gb
            gb16 = 2, //16 gb
            gb64 = 3 //64 gb
        }
    }
}