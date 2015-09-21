namespace BelaLogApplication.Logger
{
    public interface ILogService
    {
        LogLevelParameters LevelSettings { get; set; }

        void Log(LogLevelParameters level, string message);
    }
}
