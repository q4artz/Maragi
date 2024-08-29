
namespace ApiModels.Requests
{
    public class TaskAgentRequest
    {
        public string Command { get; set; }
        public string[] Arguements { get; set; }

        // for execute-assembly
        public byte[] File { get; set; }
    }
}
