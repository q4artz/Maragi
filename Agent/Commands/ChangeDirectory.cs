

using Agent.Models;
using System;
using System.IO;

namespace Agent.Commands
{
    public class ChangeDirectory : AgentCommand
    {
        public override string Name => "cd";

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

            Directory.SetCurrentDirectory(path);

            return Directory.GetCurrentDirectory();
        }
    }
}
