using Agent.Models;
using System;
using System.IO;

namespace Agent.Commands
{
    public class CreateDirectory : AgentCommand
    {
        public override string Name => "mkdir";

        public override string Execute(AgentTask task)
        {
            string path;

            if (task.Arguements is null || task.Arguements.Length == 0)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else
            {
                path = task.Arguements[0];
            }

            // path can be relative or absolute
            var dirInfo = Directory.CreateDirectory(path);

            return $"{dirInfo.FullName} created";
        }
    }
}
