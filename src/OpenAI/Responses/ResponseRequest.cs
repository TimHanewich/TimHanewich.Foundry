using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class ResponseRequest
    {
        public string Model {get; set;}
        public string? PreviousResponseID {get; set;}
        public List<Message> Messages {get; set;}
        public List<Tool> Tools {get; set;}
        public ReasoningEffortLevel? ReasoningEffort {get; set;}

        public ResponseRequest()
        {
            Model = string.Empty;
            PreviousResponseID = null;
            Tools = new List<Tool>();
            Messages = new List<Message>();
            ReasoningEffort = null;
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

            //Messages (inputs)
            if (Messages.Count > 0)
            {
                JArray messages = new JArray();
                foreach (Message msg in Messages)
                {
                    messages.Add(msg.ToJSON());
                }
                ToReturn.Add("input", messages);
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

            return ToReturn;
        }
    }
}