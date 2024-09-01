using Agent.Models;
using System.Collections.Generic;
using System.IO;

namespace Agent.Commands
{
    public class ListDirectory : AgentCommand
    {
        public override string Name => "ls";

        public override string Execute(AgentTask task)
        {
            // using Generics.cs from SharpSploit Project

            var results = new SharpSploitResultList<ListDirectoryResult>();

            string path;

            if (task.Arguements is null || task.Arguements.Length == 0) 
            {
                path = Directory.GetCurrentDirectory();
            }
            else
            {
                path = task.Arguements[0];
            }
            
            var files = Directory.GetFiles(path);
            
            foreach (var file in files) 
            {
                var fileInfo = new FileInfo(file);
                results.Add(new ListDirectoryResult
                {
                    Name = fileInfo.FullName,
                    Length = fileInfo.Length
                });
            }

            var directories = Directory.GetDirectories(path);

            foreach (var directory in directories)
            {
                var dirInfo = new DirectoryInfo(directory);
                results.Add(new ListDirectoryResult { Name = dirInfo.FullName, Length = 0 });
            }

            return results.ToString();
        }
    }

    public sealed class ListDirectoryResult : SharpSploitResult 
    {
        public string Name { get; set; }
        public long Length { get; set; }

        protected internal override IList<SharpSploitResultProperty> ResultProperties => new List<SharpSploitResultProperty>
        {
            new SharpSploitResultProperty{Name = nameof(Name), Value = Name },
            new SharpSploitResultProperty{Name = nameof(Length), Value = Length}
        };
    }
}
