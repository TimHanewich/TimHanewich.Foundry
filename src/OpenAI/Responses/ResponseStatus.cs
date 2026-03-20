using System;

namespace TimHanewich.Foundry.OpenAI.Responses
{
    public enum ResponseStatus
    {
        Unknown = 0,
        Queued = 1,
        InProgress = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
    }
}