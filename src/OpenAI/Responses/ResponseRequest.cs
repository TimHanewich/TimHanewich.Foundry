using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ResponseRequest
    {
        public string Model {get; set;}
        public string? PreviousResponseID {get; set;}
        public List<Exchange> Inputs {get; set;}
        public List<Tool> Tools {get; set;}
        public ReasoningEffortLevel? ReasoningEffort {get; set;}
        public ResponseFormat RequestedFormat {get; set;}
        public StructuredOutputSchema? OutputSchema {get; set;} //if they selected StructuredOutput as ResponseFormat, the schema they supply (can get it from https://transform.tools/json-to-json-schema)
        public bool Background {get; set;} //If it is a "background" (asynchronous) request where you submit it, it returns "got it" and you later call again to check on it.

        public ResponseRequest()
        {
            Model = string.Empty;
            PreviousResponseID = null;
            Tools = new List<Tool>();
            Inputs = new List<Exchange>(); //for messages, tool call responses, etc.
            ReasoningEffort = null;
            RequestedFormat = ResponseFormat.Text; //default to text
            Background = false;
        }

        //Prepares a responses request as a JSON object that can be sent to a Foundry OpenAI LLM service
        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Model (deployment name)
            ToReturn.Add("model", Model);

            //Previous response id
            if (PreviousResponseID != null)
            {
                ToReturn.Add("previous_response_id", PreviousResponseID);
            }

            //Inputs
            if (Inputs.Count > 0)
            {
                JArray inputs = new JArray();
                foreach (Exchange input in Inputs)
                {
                    inputs.Add(input.ToJSON());
                }
                ToReturn.Add("input", inputs);
            }
            
            //Tools
            if (Tools.Count > 0)
            {
                JArray tools = new JArray();
                foreach (Tool tool in Tools)
                {
                    tools.Add(tool.ToJSON());
                }
                ToReturn.Add("tools", tools);
            }

            //Reasoning effort
            if (ReasoningEffort != null)
            {
                JObject effort = new JObject();
                if (ReasoningEffort == ReasoningEffortLevel.Low)
                {
                    effort.Add("effort", "low");
                }
                else if (ReasoningEffort == ReasoningEffortLevel.Medium)
                {
                    effort.Add("effort", "medium");
                }
                else if (ReasoningEffort == ReasoningEffortLevel.High)
                {
                    effort.Add("effort", "high");
                }
                ToReturn.Add("reasoning", effort);
            }

            //Requested format
            if (RequestedFormat == ResponseFormat.Text)
            {
                //do nothing! By default, it is in text. Do nothing to save bandwidth on the call.
            }
            else if (RequestedFormat == ResponseFormat.JsonObject)
            {
                JObject type = new JObject();
                type.Add("type", "json_object");
                JObject format = new JObject();
                format.Add("format", type);
                ToReturn.Add("text", format);
            }
            else if (RequestedFormat == ResponseFormat.StructuredOutput)
            {
                if (OutputSchema == null)
                {
                    throw new Exception("You specified '" + RequestedFormat.ToString() + "' as the output format but did NOT provide a StructuredOutputSchema. That must be provided.");
                }
                JObject jo_text = new JObject();
                jo_text.Add("format", OutputSchema.ToJSON());
                ToReturn.Add("text", jo_text);
            }

            //Background
            ToReturn.Add("background", Background);

            return ToReturn;
        }
    }
}