using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Maragi_Framework.Models
{
    public class HttpListener : Listener
    {
        // Unique Property for HttpListener
        public override string Name { get; }
        public int BindPort { get; }

        private CancellationTokenSource _tokenSource;

        public HttpListener(string name, int bindPort)
        {
            Name = name;
            BindPort = bindPort;
        }

        public override async Task Start()
        {
            // use default web server capabilities
            var hostBuilder = new HostBuilder()
                .ConfigureWebHostDefaults(host =>
                {
                    // ASP.NET core is heavy with services -- we need it for performing taksing/handling implants
                    host.UseUrls($"http://0.0.0.0:{BindPort}");
                    host.Configure(ConfigureApp);
                    host.ConfigureServices(ConfigureServices);
                });

            var host = hostBuilder.Build();

            _tokenSource = new CancellationTokenSource();

            // takes cancelation token
            // Run for as long as token is not cancelled
            host.RunAsync(_tokenSource.Token);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(AgentService);
        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                // Configure which URIs for TeamServer to respond on.
                // able to build in malleable C2 profile thing into Listener.
                // can be changed to for exp -- "/index.php" -- so TS only response when this page is called.
                e.MapControllerRoute("/", "/", new { controller = "HttpListener", action = "HandleImplant" });
            });
        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}