namespace Maragi_Framework.Models.Agent_Implants
{
    // task definitions
    public class AgentTask
    {
        public string Id { get; set; }

        public string Command  { get; set; }
        public string[] Arguements { get; set; }

        // for execute-assembly
        public byte[] File { get; set; }
    }
}
