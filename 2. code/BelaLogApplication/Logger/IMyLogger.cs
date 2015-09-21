namespace BelaLogApplication.Logger
{
    public interface IMyLogger
    {
        void Configure(LogTargetParameters targetParameters, LogLevelParameters levelParameters);

        void LogMessage(string message, LogLevelParameters levelParameters);
    }
}