using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using KnightFrank.Hub.LandRegistry.Common;
using KnightFrank.Hub.LandRegistry.Service;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace KnightFrank.Hub.LandRegistry.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWebApplication(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()

                .ConfigureAppConfiguration(config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();

                    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                    {
                        var options = new OpenApiConfigurationOptions()
                        {
                            Info = new OpenApiInfo()
                            {
                                Version = "1.0.0",
                                Title = "Land Registry",
                                Description = "Land Registry API",
                                TermsOfService = new Uri("https://www.knightfrank.com/legals"),
                                Contact = new OpenApiContact()
                                {
                                    Name = "Contact",
                                    Url = new Uri("https://www.knightfrank.com/contact"),
                                }
                            },
                            Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                            OpenApiVersion = OpenApiVersionType.V3,
                            IncludeRequestingHostName = true,
                            ForceHttps = false,
                            ForceHttp = false
                        };

                        return options;
                    });

                    services.AddHttpClient();
                    services.AddLogging();

                    services.AddCoreServices();
                    services.AddLandRegistryServices();
                    services.Configure<KestrelServerOptions>(options =>
                    {
                        options.AllowSynchronousIO = true;
                    });

                })
                .Build();

            host.Run();
        }
    }
}

