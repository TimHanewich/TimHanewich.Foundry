using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public class Message : Exchange
    {
        public Role Role {get; set;}
        public string? Text {get; set;}

        public Message()
        {
            
        }

        public Message(Role role, string text)
        {
            Role = role;
            Text = text;
        }

        public JObject ToJSON()
        {
            JObject ToReturn = new JObject();

            //Role
            if (Role == Role.developer)
            {
                ToReturn.Add("role", "developer");
            }
            else if (Role == Role.user)
            {
                ToReturn.Add("role", "user");    
            }
            else if (Role == Role.assistant)
            {
                ToReturn.Add("role", "assistant");    
            }

            //Text
            if (Text != null)
            {
                ToReturn.Add("content", Text);
            }

            return ToReturn;
        }

        public static Message Parse(JObject msg)
        {
            Message ToReturn = new Message();

            //get role
            JProperty? role = msg.Property("role");
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
            }

            //Get text Text
            JToken? output_text = msg.SelectToken("content[0].text");
            if (output_text != null)
            {
                ToReturn.Text = output_text.ToString();
            }

            return ToReturn;
        }
    }
}