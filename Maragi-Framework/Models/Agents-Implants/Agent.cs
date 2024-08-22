using System;

namespace Maragi_Framework.Models
{
    public class Agent
    {
        // AgentMetadata from AgentMetadata.cs
        public AgentMetadata Metadata { get;}
        public DateTime LastSeen { get; private set; }

        // Constructing Agent / Implant
        public Agent(AgentMetadata metadata) 
        {     
            Metadata = metadata;
        }

        public void CheckIn() {
            LastSeen = DateTime.UtcNow;
        }

        // blank IEnumeratble
        public void GetPendingtask()
        {

        }
    }
}
