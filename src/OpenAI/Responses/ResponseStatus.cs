using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    //See https://i.imgur.com/wJTg2Dj.png
    //From https://developers.openai.com/api/reference/python/resources/responses
    public enum ResponseStatus
    {
        Unknown = 0,
        Queued = 1,
        InProgress = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Incomplete = 6      //happens if an input or output was blocked
    }
}