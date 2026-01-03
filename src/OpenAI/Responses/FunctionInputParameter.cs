using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class FunctionInputParameter
    {
        public string Name {get; set;} //The name of the input parameter (JSON property name)
        public string ParameterType {get; set;} //i.e. string, number
        public string Description {get; set;} //Description of what this parameter is
        public bool Required {get; set;} //if this is a required parameter or not (means the LLM must fill it out)

        public FunctionInputParameter()
        {
            Name = "";
            ParameterType = "string";
            Description = "";
            Required = true;
        }

        public FunctionInputParameter(string name, string description, string parameter_type = "string")
        {
            Name = name;
            Description = description;
            ParameterType = parameter_type;
        }
    }
}