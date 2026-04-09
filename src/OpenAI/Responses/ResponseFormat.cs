using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public enum ResponseFormat
    {
        Text = 0,
        JsonObject = 1,      //just a JSON object you give an example of.
        StructuredOutput = 2 //must provide JSON schema it adheres to
    }
}