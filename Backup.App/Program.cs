using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Backup.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string local = "Backup.local";
            const string emails = "Backup.emails";
            const string accessKey = "Aws.accessKey";
            const string secretKey = "Aws.secretKey";
            const string bucketName = "Aws.bucketName";

            var log = new StringBuilder();
            log.AppendFormat("Backup iniciado {0:dd\\/MM\\/yyy hh\\:mm\\:ss}\n", DateTime.Now);
            try
            {
                foreach (var file in Directory.GetFiles(ConfigurationManager.AppSettings[local]))
                {
                    File.Delete(file);
                }

                foreach (ConnectionStringSettings str in ConfigurationManager.ConnectionStrings)
                {
                    var arquivo = new BackupSqlServer(log,
                                                      str.ConnectionString,
                                                      ConfigurationManager.AppSettings[local])
                        .RealizarBackup();

                    new EnvioS3(log,
                                arquivo,
                                ConfigurationManager.AppSettings[accessKey],
                                ConfigurationManager.AppSettings[secretKey],
                                ConfigurationManager.AppSettings[bucketName])
                        .Enviar(true);
                }

                log.AppendFormat("Backup finalizado com sucesso {0:dd\\/MM\\/yyy hh\\:mm\\:ss}\n", DateTime.Now);
            }
            catch(Exception ex)
            {
                log.AppendLine("Falha durante o backup");
                log.AppendLine(ex.Message);
                log.Append(ex.StackTrace);
            }

            new EnvioEmail(log,
                           ConfigurationManager.AppSettings[accessKey],
                           ConfigurationManager.AppSettings[secretKey],
                           ConfigurationManager.AppSettings[emails])
                .Enviar();
        }
    }
}
