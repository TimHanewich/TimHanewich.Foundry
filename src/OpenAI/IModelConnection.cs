using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimHanewich.AgentFramework.Responses
{
    public interface IModelConnection
    {
        public Task<InferenceResponse> InvokeInferenceAsync(Message[] inputs, Tool[] tools, bool json_mode);
    }
}