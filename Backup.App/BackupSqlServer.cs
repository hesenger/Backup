using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Backup.App
{
    public class BackupSqlServer
    {
        private readonly StringBuilder _log;
        private readonly string _connectionString;
        private readonly string _path;
        
        public BackupSqlServer(StringBuilder log, string connectionString, string path)
        {
            _log = log;
            _connectionString = connectionString;
            _path = path;
        }
        
        public string RealizarBackup()
        {
            const string template = "DBCC SHRINKDATABASE(N'{0}');\n" +
                                    "BACKUP DATABASE [{0}] " +
                                   "TO DISK = N'{1}' " +
                                   "WITH NOFORMAT, NOINIT, SKIP, NAME = N'{0} - {2:dd/MM/yyyy hh:mm}';";
            
            var now = DateTime.Now;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var database = connection.Database;
                var file = Path.Combine(_path, database + now.ToString("-yyyy-MM-dd-hh-mm.bak"));
                var sql = string.Format(template, database, file, now);
                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                _log.AppendFormat("Backup {0} feito em {1:hh\\:mm\\:ss\\.fff}\n",
                    database,
                    DateTime.Now.Subtract(now));
                return file;
            }
        }
    }
}
