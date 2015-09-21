using System;

namespace BelaLogApplication.Logger
{
    public class MyLogger : IMyLogger
    {
        public const string InvalidConfigurationExceptionMessage = "Invalid configuration, at least one target must be specified.";

        public const string InvalidLevelExceptionMessage = "Error or Warning or Message level must be specified.";

        private static MyLogger _instance;

        private static readonly object PadLock = new object();

        private readonly ILogDatabaseService _databaseService;

        private readonly ILogFileService _fileService;

        private readonly ILogConsoleService _consoleService;

        private LogTargetParameters _targetSettings;

        private LogLevelParameters _levelSettings;

        public static MyLogger Instance
        {
            get
            {
                lock (PadLock)
                {
                    return _instance ?? (_instance = new MyLogger());
                }
            }
        }

        public MyLogger() : this(new LogDatabaseService(), new LogFileService(), new LogConsoleService())
        {
        }

        public MyLogger(ILogDatabaseService databaseService, ILogFileService fileService, ILogConsoleService consoleService)
        {
            _databaseService = databaseService;
            _fileService = fileService;
            _consoleService = consoleService;
        }

        public void Configure(LogTargetParameters targetParameters, LogLevelParameters levelParameters)
        {
            _targetSettings = targetParameters;
            _levelSettings = levelParameters;
            _databaseService.LevelSettings = _levelSettings;
            _fileService.LevelSettings = _levelSettings;
            _consoleService.LevelSettings = _levelSettings;
        }

        public void LogMessage(string message, LogLevelParameters levelParameters)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            message = message.Trim();

            if (_targetSettings == null 
                || (_targetSettings != null && !_targetSettings.Console && !_targetSettings.File && !_targetSettings.DataBase))
            {
                throw new Exception(InvalidConfigurationExceptionMessage);
            }

            if (_levelSettings == null 
                || (_levelSettings != null && !_levelSettings.Error && !_levelSettings.Message && !_levelSettings.Warning) 
                || (levelParameters != null && !levelParameters.Message && !levelParameters.Warning && !levelParameters.Error))
            {
                throw new Exception(InvalidLevelExceptionMessage);
            }

            if (_targetSettings.DataBase)
            {
                _databaseService.Log(levelParameters, message);
            }

            if (_targetSettings.File)
            {
                _fileService.Log(levelParameters, message);
            }

            if (_targetSettings.Console)
            {
                _consoleService.Log(levelParameters, message);
            }
        }
    }
}
