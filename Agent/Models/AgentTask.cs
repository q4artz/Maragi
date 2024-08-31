using System.Runtime.Serialization;
namespace Agent.Models
{

    [DataContract]
    // task definitions
    public class AgentTask
    {
        [DataMember(Name  = "id")]
        public string Id { get; set; }

        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "arguements")]
        public string[] Arguements { get; set; }

        [DataMember(Name = "file")]
        // for execute-assembly
        public byte[] File { get; set; }
    }
}
