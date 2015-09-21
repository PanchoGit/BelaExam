using System;
using System.IO;

namespace BelaLogApplication.Logger
{
    public class LogFileService : ILogFileService
    {
        private string LogFileDirectory { get; set; }

        public LogLevelParameters LevelSettings { get; set; }

        public LogFileService()
        {
            LogFileDirectory = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"];
        }

        public void Log(LogLevelParameters level, string message)
        {
            var line = string.Empty;

            var logFileName = Path.Combine(LogFileDirectory, string.Format("LogFile{0:d}.txt", DateTime.Now));

            if (!File.Exists(logFileName))
            {
                line = File.ReadAllText(logFileName);
            }

            File.WriteAllText(logFileName, string.Format("{0}{1:d} {2}", line, DateTime.Now, message));
        }
    }
}
