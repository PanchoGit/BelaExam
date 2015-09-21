using System;
using BelaLogApplication.Logger;

namespace BelaLogApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteMyLogger();
        }

        private static void ExecuteMyLogger()
        {
            Console.ReadKey();

            try
            {
                var target = new LogTargetParameters {Console = true};
                var level = new LogLevelParameters { Error = true, Message = true, Warning = true };
                MyLogger.Instance.Configure(target, level);
                MyLogger.Instance.LogMessage(" Logger.init  ", new LogLevelParameters { Error = true, Message = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
