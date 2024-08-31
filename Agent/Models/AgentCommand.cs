namespace Agent.Models
{
    public abstract class AgentCommand
    {
        public string Name { get; }

        public abstract string Execute(AgentTask task);
    }
}
