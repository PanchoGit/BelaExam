using System;

namespace BelaLogApplication.Logger
{
    public class LogConsoleService : ILogConsoleService
    {
        public LogLevelParameters LevelSettings { get; set; }

        public void Log(LogLevelParameters levelParameters, string message)
        {
            if (LevelSettings.Error && levelParameters.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (LevelSettings.Warning && levelParameters.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (LevelSettings.Message && levelParameters.Message)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("{0:d} {1}", DateTime.Now, message);
        }
    }
}
