using System;
using System.Text;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Backup.App
{
    public class EnvioEmail
    {
        private readonly StringBuilder _log;
        private readonly string[] _destinos;
        private readonly AmazonSimpleEmailServiceClient _client;

        public EnvioEmail(StringBuilder log, string accessKey, string secretKey, string destinos)
        {
            _log = log;
            _destinos = destinos.Split(',');
            _client = new AmazonSimpleEmailServiceClient(accessKey, secretKey);
        }

        public void Enviar()
        {
            var body = new Body().WithText(new Content(_log.ToString()));
            var mess = new Message(
                new Content("Backup " + DateTime.Now.ToString("dd/MM/yyyy hh:mm")),
                body);
            var req = new SendEmailRequest("sistemas@fabrica7.net",
                                           new Destination().WithToAddresses(_destinos),
                                           mess);

            try
            {
                _client.SendEmail(req);
            }
            catch
            {
            }
        }
    }
}