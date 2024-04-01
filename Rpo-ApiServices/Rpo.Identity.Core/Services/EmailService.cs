using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage identityMessage)
        {
            await ConfigurateSendAsync(identityMessage);
        }

        private async Task ConfigurateSendAsync(IdentityMessage identityMessage)
        {
            await Task.Run(() =>
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("teste@credencys.com", "Credencys");
                mail.To.Add(identityMessage.Destination);
                mail.Subject = identityMessage.Subject;
                mail.Body = identityMessage.Body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.EnableSsl = false;

                client.UseDefaultCredentials = false;
                client.Host = "192.168.2.40";
                client.Credentials = new NetworkCredential("user", "password");

                //Add this line to bypass the certificate validation
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                client.Send(mail);
            });
        }
    }
}