using System.IO;
using System.IO.Compression;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;

namespace Backup.App
{
    public class EnvioS3
    {
        private readonly StringBuilder _log;
        private readonly string _arquivo;
        private readonly string _bucketName;
        private readonly AmazonS3Client _client;
        
        public EnvioS3(StringBuilder log, string arquivo, string accessKey, string secretKey, string bucketName)
        {
            _log = log;
            _arquivo = arquivo;
            _bucketName = bucketName;
            _client = new AmazonS3Client(accessKey, secretKey);
        }
        
        public void Enviar(bool compactar)
        {
            const string mime = "application/octet-stream";
            var nome = _arquivo.Substring(_arquivo.LastIndexOf('\\') + 1) + ".gz";
            var mem = new MemoryStream();

            using (var stream = File.OpenRead(_arquivo))
            using (var gz = new GZipStream(mem, CompressionMode.Compress, true))
            {
                stream.CopyTo(gz);
            }

            var len = mem.Length;
            var req = new PutObjectRequest()
                .WithContentType(mime)
                .WithBucketName(_bucketName)
                .WithKey(nome)
                .WithCannedACL(S3CannedACL.Private)
                .WithAutoCloseStream(true);
            
            req.InputStream = mem;
            _client.PutObject(req);
            
            _log.AppendFormat("Envio do arquivo {0} de {1} KB\n",
                              nome,
                              (len / 1024f));
        }
    }
}