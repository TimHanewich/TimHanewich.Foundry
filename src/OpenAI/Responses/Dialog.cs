using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.AgentFramework.Responses
{
    public class Message : Exchange
    {
        public Role Role {get; set;}
        public string? Content {get; set;}

        public Message()
        {
            
        }

        public Message(Role role, string content)
        {
            Role = role;
            Content = content;
        }

        public static Message Parse(JObject Message)
        {
            Message ToReturn = new Message();

            //get role
            JProperty? role = Message.Property("role");
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
            JProperty? content = Message.Property("content");
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