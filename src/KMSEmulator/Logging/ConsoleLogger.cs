using System;

namespace KMSEmulator.Logging
{
    public class ConsoleLogger : ILogger
    { 
        public void LogMessage(string message, bool timestamp = true)
        {
            if (timestamp)
            {
                Console.WriteLine(DateTime.Now.ToString("s") + "\t" + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
