using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public enum ResponseFormat
    {
        Text = 0,
        JsonObject = 1
        //"JsonSchema" would be 2, but that isn't supported in this library yet.
    }
}