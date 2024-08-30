using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Models
{
    internal class HttpCommModule : CommModule
    {
        public string ConnectAddress { get; set; }
        public int ConnectPort { get; set; }

        private CancellationTokenSource _tokenSource;

        private HttpClient _client;

        public HttpCommModule(string connectAddress, int connectPort) {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
        }

        public override void Init(AgentMetadata metadata)
        {
            base.Init(metadata);

            _client = new HttpClient();
            _client.BaseAddress = new System.Uri($"{ConnectAddress}:{ConnectPort}");
            _client.DefaultRequestHeaders.Clear();

            // Serealize and add in out agent metadata
        }

        public override Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                // checkin

                // get tasks

                // sleep
            }
        }

        public void Checkin()
        {

        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
