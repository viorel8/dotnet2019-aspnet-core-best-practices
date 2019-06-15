using DotNet2019.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet2019.Host
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; set; }
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.ConfigureServices(services, Environment);

            services
            .AddCustomHealthChecks()
            .AddHostingDiagnosticHandler();
            
        }

        public void Configure(IApplicationBuilder app)
        {
            Configuration.Configure(app, host =>
            {
                return host
                    .UseCustomHealthchecks()
                    .UseHeaderDiagnostics();
            });
        }
    }
}
