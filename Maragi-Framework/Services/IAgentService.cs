using Maragi_Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Maragi_Framework.Services
{
    // resembles IListenerService closely
    public interface IAgentService
    {
        void AddAgent(Agent agent);
        // can return a list / array / any type
        IEnumerable<Agent> GetAgents();
        Agent GetAgent(string id);
        void RemoveAgent(Agent agent);
    }
    public class AgentService : IAgentService
    {
        private readonly List<Agent> _agents = new();
        public void AddAgent(Agent agent)
        {
            _agents.Add(agent);
        }

        public IEnumerable<Agent> GetAgents()
        {
            return _agents;
        }

        public Agent GetAgent(string id)
        {
            return GetAgents().FirstOrDefault(a => a.Metadata.Id.Equals(id));
        }

        public void RemoveAgent(Agent agent)
        {
           _agents.Remove(agent);
        }
    }
}
