using System;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class CodeInterpreterCall : Exchange
    {
        public string Code {get; set;}

        public CodeInterpreterCall()
        {
            Code = string.Empty;
        }

        public static CodeInterpreterCall Parse(JObject cic)
        {
            CodeInterpreterCall ToReturn = new CodeInterpreterCall();

            //Get code
            JProperty? prop_code = cic.Property("code");
            if (prop_code != null)
            {
                ToReturn.Code = prop_code.Value.ToString();
            }

            return ToReturn;
        }
    }
}