using System;
using System.Data.SqlClient;
using System.Text;

namespace Backup.App
{
    public class CheckSqlServer
    {
        private readonly StringBuilder _log;
        private readonly string _connectionString;

        public CheckSqlServer(StringBuilder log, string connectionString)
        {
            _log = log;
            _connectionString = connectionString;
        }

        public void RealizarCheck()
        {
            const string template = "DBCC CHECKDB(N'{0}') WITH NO_INFOMSGS, ALL_ERRORMSGS";
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.InfoMessage += ConnectionOnInfoMessage;

                var database = connection.Database;
                _log.AppendLine("Check " + database);

                var sql = string.Format(template, database);
                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ConnectionOnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Message) && args.Errors.Count == 0)
                return;

            _log.Append(args.Message);
            foreach (SqlError error in args.Errors)
            {
                _log.AppendLine(error.Message);
            }
        }
    }
}
