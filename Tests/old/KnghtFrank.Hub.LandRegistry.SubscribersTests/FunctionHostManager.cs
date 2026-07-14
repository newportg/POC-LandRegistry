using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace KnghtFrank.Hub.LandRegistry.SubscribersTests
{
    public class FunctionHostManager
    {
        private const string FunctionHostPath = @"%USERPROFILE%\AppData\Local\AzureFunctionsTools\Releases\3.18.0\cli_x64\func.exe";
        private static Process _functionHostProcess;

        public static string FunctionAppRootPath
        {
            get
            {
#if DEBUG
                return @"..\..\..\..\KnghtFRank.Hub.LandRegistry.Subscribers\bin\Debug\netcoreapp3.1";
#else
                return @"..\..\..\..\KnghtFRank.Hub.LandRegistry.Subscribers\bin\Release\netcoreapp3.1";
#endif
            }
        }

        public static void StartHost()
        {
            var functionHostPath = Environment.ExpandEnvironmentVariables(FunctionHostPath);
            var functionAppRootPath = Path.GetFullPath(FunctionAppRootPath);

            ProcessStartInfo startInfo = new ProcessStartInfo(functionHostPath)
            {
                WorkingDirectory = functionAppRootPath,
                Arguments = "host start"
            };

            _functionHostProcess = Process.Start(startInfo);
            Thread.Sleep(new TimeSpan(0, 0, 10));
        }

        public static void StopHost()
        {
            _functionHostProcess.Kill();
        }
    }
}
