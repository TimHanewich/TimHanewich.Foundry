using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public enum ResponseStatus
    {
        Queued = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}