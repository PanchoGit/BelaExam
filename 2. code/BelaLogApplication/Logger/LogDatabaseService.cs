using System.Data.SqlClient;

namespace BelaLogApplication.Logger
{
    public class LogDatabaseService : ILogDatabaseService
    {
        private string ConnectionString { get; set; }

        public LogLevelParameters LevelSettings { get; set; }

        public LogDatabaseService()
        {
            ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }

        public void Log(LogLevelParameters level, string message)
        {
            var connection = new SqlConnection(ConnectionString, null);
            connection.Open();

            var levelCode = 0;
            if (LevelSettings.Message && level.Message)
            {
                levelCode = 1;
            }
            if (LevelSettings.Error && level.Error)
            {
                levelCode = 2;
            }
            if (level.Warning && level.Warning)
            {
                levelCode = 3;
            }

            var command = new SqlCommand("INSERT INTO Log VALUES('@message', @levelCode)");
            command.Parameters.Add(new SqlParameter("@message", message));
            command.Parameters.Add(new SqlParameter("@levelCode", levelCode));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
