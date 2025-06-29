using System;
using System.IO;

namespace MauiApp2.Services
{
    public class LogService
    {
        private readonly string logFilePath;

        public LogService(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void LogAction(string action)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {action}{Environment.NewLine}";
            File.AppendAllText(logFilePath, logEntry);
        }

        public string LogFilePath => logFilePath;
    }
}