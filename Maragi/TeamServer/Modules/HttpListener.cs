


namespace TeamServer.Modules
{
    public class HttpListener : Listener
    {
        public override string Name { get; }

        // Unique Property for HttpListener
        public int BindPort { get;}

        private CancellationTokenSource _tokenSource;

        public HttpListener(string name, int bindPort) {
            Name = name;
            BindPort = bindPort;
        }

        public override async Task Start()
        {
            // use default web server capabilities
            var HostBuilder = new HostBuilder()
                 .ConfigureWebHostDefaults(Host =>
                 {
                     // ASP.NET core is heavy with services -- we need it for performing taksing/handling implants
                     Host.UseUrls($"http://0.0.0.0{BindPort}");
                     Host.Configure(ConfigureApp);
                     Host.ConfigureServices(configureServices);
                 });
            
            var host = HostBuilder.Build();

            _tokenSource = new CancellationTokenSource();

            // takes cancelation token
            // Run for as long as token is not cancelled
            host.RunAsync(_tokenSource.Token);
        }

        private void configureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(e =>
            {
                // Configure which URIs for TeamServer to respond on.
                // able to build in malleable C2 profile thing into Listener.
                // can be changed to for exp -- "/index.php" -- so TS only response when this page is called.
                e.MapControllerRoute("/","/", new { controller = "HttpListener", action = "HandleImplant"});
            });
        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
