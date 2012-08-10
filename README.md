Backup
======

Aplicação que faz backup de bases de dados Sql Server e as envia
para o Amazon S3 e envia o log das operações realizadas aos e-mails
listados na configuração. O envio dos e-mails é feito através do
Amazon SES.

Para que seja executado periodicamente a aplicação pode ser agendada
utilizando-se a agenda padrão do Windows.


Configuração
------------

As configurações necessárias são feitas no arquivo `.config` que estará
disponível com a aplicação. São necessárias as informações de chave da
conta AWS.

As configurações são as seguintes:
- Strings de conexão, podem ser utilizadas quantas forem necessárias,
sendo uma para cada banco;
- Local onde o serviço do Sql Server tenha permissões de gravar os backups;
- E-mails para envio do log;
- Chave de acesso a conta AWS;
- Chave secreta de acesso a conta AWS;
- Nome do bucket no Amazon S3 para armazenagem dos backups.