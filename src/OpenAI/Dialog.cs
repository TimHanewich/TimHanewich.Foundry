using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.AgentFramework.Responses
{
    public class Dialog : Message
    {
        public Role Role {get; set;}
        public string? Content {get; set;}

        public Dialog()
        {
            
        }

        public Dialog(Role role, string content)
        {
            Role = role;
            Content = content;
        }

        public static Dialog Parse(JObject Dialog)
        {
            Dialog ToReturn = new Dialog();

            //get role
            JProperty? role = Dialog.Property("role");
            if (role != null)
            {
                string rolestr = role.Value.ToString();
                if (rolestr == "system" || rolestr == "developer")
                {
                    ToReturn.Role = Role.developer;
                }
                else if (rolestr == "user")
                {
                    ToReturn.Role = Role.user;
                }
                else if (rolestr == "assistant")
                {
                    ToReturn.Role = Role.assistant;
                }
                else if (rolestr == "tool")
                {
                    ToReturn.Role = Role.tool;
                }
            }

            //Get content
            JProperty? content = Dialog.Property("content");
            if (content != null)
            {
                if (content.Value.Type != JTokenType.Null)
                {
                    ToReturn.Content = content.Value.ToString();
                }
            }

            return ToReturn;
        }
    
    }
}