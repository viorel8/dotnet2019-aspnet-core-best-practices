using DotNet2019.Api.Infrastructure.Data;
using DotNet2019.Api.Infrastructure.Hubs;
using DotNet2019.Api.Infrastructure.Middleware;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNet2019.Api
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            return services

                .AddHttpContextAccessor()
                .AddCustomMvc()
                .AddCustomMiddlewares()
                .AddCustomProblemDetails(environment)
                .AddCustomApiBehaviour()
                .AddCustomServices()
                .AddSignalR()
                .Services;
                
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> configureHost)
        {
            return configureHost(app)                
                .UseProblemDetails()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseMiddleware<ErrorMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();

                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync($"Hello DotNet 2019! from {System.Environment.MachineName}");
                    });

                    endpoints.MapSecretEndpoint().RequireAuthorization("ApiKeyPolicy");

                    endpoints.MapHub<DiagnosticsHub>("/diagnostics-hub");
                });
        }
    }
}
