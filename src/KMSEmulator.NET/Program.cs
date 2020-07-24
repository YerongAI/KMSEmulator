using System;
using System.Runtime.Loader;
using System.Threading;
using KMSEmulator.Logging;

namespace KMSEmulator.NET
{
	class Program
    {
        private static readonly ConsoleLogger Logger = new ConsoleLogger();
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            RegisterHandlers();

            // Set KMS Server Settings
            KMSServerSettings kmsSettings = new KMSServerSettings
			{
				KillProcessOnPort = false,
				GenerateRandomKMSPID = true,
				DefaultKMSHWID = "364F463A8863D35F"
			};

			// Start KMS Server
			KMSServer.Start(Logger, kmsSettings);
            QuitEvent.WaitOne();
            Logger.LogMessage("Service is stopped.");
        }

        #region

        private static void RegisterHandlers()
        {
            try
            {
                AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
                Console.CancelKeyPress += SigIntEventHandler;
                AppDomain.CurrentDomain.UnhandledException += UnhandledHandler;
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Cannot register system event handlers");
                Logger.LogMessage(ex.StackTrace);
            }
        }

        private static void UnhandledHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.LogMessage($"Fatal error, {(e.ExceptionObject as Exception)?.Message}");
            Logger.LogMessage((e.ExceptionObject as Exception)?.StackTrace);
        }

        private static void SigIntEventHandler(object sender, ConsoleCancelEventArgs e)
        {
            Logger.LogMessage("Exiting...");
            QuitEvent.Set();
        }

        private static void SigTermEventHandler(AssemblyLoadContext obj)
        {
            Logger.LogMessage("Unloading...");
            QuitEvent.Set();
        }

        #endregion

    }
}
