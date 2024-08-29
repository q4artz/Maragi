using Maragi_Framework.Services;
using System.Threading.Tasks;

namespace Maragi_Framework.Models.Listeners
{
    public abstract class Listener
    {
        public abstract string Name { get; }

        protected IAgentService AgentService;
        public void Init(IAgentService agentService)
        {
            AgentService = agentService;
        }

        // Starting Listener
        public abstract Task Start();

        // Stopping Listener
        public abstract void Stop();
    }
}
