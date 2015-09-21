using System;
using BelaLogApplication.Logger;
using Moq;
using NUnit.Framework;

namespace Unit
{
    [TestFixture]
    public class LoggerTest
    {
        const string ValidMessage = "blah";

        private Mock<ILogDatabaseService> _databaseServiceMock;

        private Mock<ILogFileService> _fileServiceMock;

        private Mock<ILogConsoleService> _consoleServiceMock;

        private MyLogger _sut;

        [SetUp]
        public void Setup()
        {
            _databaseServiceMock = new Mock<ILogDatabaseService>();

            _fileServiceMock = new Mock<ILogFileService>();

            _consoleServiceMock = new Mock<ILogConsoleService>();

            _sut = new MyLogger(_databaseServiceMock.Object, _fileServiceMock.Object, _consoleServiceMock.Object);
        }

        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void ShouldSuccessfullyInvokeSpecificLogService(bool isDatabase, bool isFile, bool isConsole)
        {

            var logLevelParms = new LogLevelParameters { Message = true, Warning = true, Error = true };

            _databaseServiceMock.Setup(s => s.Log(logLevelParms, ValidMessage));

            _fileServiceMock.Setup(s => s.Log(logLevelParms, ValidMessage));

            _consoleServiceMock.Setup(s => s.Log(logLevelParms, ValidMessage));

            var targetParameters = new LogTargetParameters
            {
                DataBase = isDatabase,
                File = isFile,
                Console = isConsole
            };
            
            _sut.Configure(targetParameters, new LogLevelParameters { Message = true });

            _sut.LogMessage(ValidMessage, logLevelParms);

            _databaseServiceMock.Verify(s => s.Log(logLevelParms, ValidMessage), isDatabase ? Times.Once() : Times.Never());

            _fileServiceMock.Verify(s => s.Log(logLevelParms, ValidMessage), isFile ? Times.Once() : Times.Never());

            _consoleServiceMock.Verify(s => s.Log(logLevelParms, ValidMessage), isConsole ? Times.Once() : Times.Never());
        }

        [Test]
        public void ShouldThrowExceptionWithNullSettings()
        {
            var logLevelParms = new LogLevelParameters { Message = true, Warning = true, Error = true };

            var actual = Assert.Throws<Exception>(() =>
            {
                _sut.LogMessage(ValidMessage, logLevelParms);
            });

            Assert.AreSame(MyLogger.InvalidConfigurationExceptionMessage, actual.Message);
        }

        [Test]
        public void ShouldThrowExceptionWithAllFalseTargetSettings()
        {
            var logLevelParms = new LogLevelParameters { Message = false, Warning = false, Error = false };

            _sut.Configure(new LogTargetParameters(), new LogLevelParameters{ Message = true });

            var actual = Assert.Throws<Exception>(() =>
            {
                _sut.LogMessage(ValidMessage, logLevelParms);
            });

            Assert.AreSame(MyLogger.InvalidConfigurationExceptionMessage, actual.Message);
        }

        [Test]
        public void ShouldThrowExceptionWithAllFalseLevelSettings()
        {
            var logLevelParms = new LogLevelParameters { Message = true, Warning = true, Error = true };

            _sut.Configure(new LogTargetParameters{ Console = true }, new LogLevelParameters());

            var actual = Assert.Throws<Exception>(() =>
            {
                _sut.LogMessage(ValidMessage, logLevelParms);
            });

            Assert.AreSame(MyLogger.InvalidLevelExceptionMessage, actual.Message);
        }
    }
}
