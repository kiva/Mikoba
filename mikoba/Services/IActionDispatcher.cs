using System.Threading.Tasks;
using Hyperledger.Aries.Agents;

namespace mikoba.Services
{
    public interface IActionDispatcher
    { 
        Task DispatchMessage(AgentMessage message);
    }
}
