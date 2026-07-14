using System;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace KnightFrank.Hub.LandRegistry.ApiTests
{
    public class AzureFunctionRunner : IAsyncDisposable
    {
        private readonly Process _application;
        private static readonly HttpClient HttpClient = new ();

        private AzureFunctionRunner(Process application)
        {
            _application = application;
        }

        public static async Task<AzureFunctionRunner> StartNewAsync(DirectoryInfo projectDirectory)
        {
            int port = Int32.Parse(Environment.GetEnvironmentVariable("FunctionPortNo"));
            Process app = StartApplication(port, projectDirectory);
            await WaitUntilTriggerIsAvailableAsync($"http://localhost:{port}/");

            return new AzureFunctionRunner(app);
        }

        private static Process StartApplication(int port, DirectoryInfo projectDirectory)
        {
            var appInfo = new ProcessStartInfo("func", $"start --port {port} --prefix bin/Debug/net8.0")
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                WorkingDirectory = projectDirectory.FullName
            };

            var app = new Process { StartInfo = appInfo };
            app.Start();
            return app;
        }

        private static async Task WaitUntilTriggerIsAvailableAsync(string endpoint)
        {
            AsyncRetryPolicy retryPolicy =
                    Policy.Handle<Exception>()
                          .WaitAndRetryForeverAsync(index => TimeSpan.FromMilliseconds(500));

            PolicyResult<HttpResponseMessage> result =
                await Policy.TimeoutAsync(TimeSpan.FromSeconds(30))
                            .WrapAsync(retryPolicy)
                            .ExecuteAndCaptureAsync(() => HttpClient.GetAsync(endpoint));

            if (result.Outcome == OutcomeType.Failure)
            {
                throw new InvalidOperationException(
                    "The Azure Functions project doesn't seem to be running, "
                    + "please check any build or runtime errors that could occur during startup");
            }
        }

        public ValueTask DisposeAsync()
        {
            if (!_application.HasExited)
            {
                _application.Kill(entireProcessTree: true);
            }

            _application.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}