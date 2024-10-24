using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Drawing.Imaging;
using System.Drawing;
using Acelera.API.AuthOriginal.Repository.Interface;
using Acelera.API.AuthOriginal.Helpers;
using Microsoft.Extensions.Options;

namespace Acelera.API.AuthOriginal.Repository
{
    public class MailRepository : IMailRepository
    {
        private readonly EmailSettings _emailSettings;

        public MailRepository(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public bool SendEmail(string destinatario, string emailAssunto, string emailCorpo)
        {
            var retorno = false;

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailSettings.EmailUsername));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = emailAssunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailCorpo
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.EmailHost, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.EmailUsername, _emailSettings.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);

                retorno = true;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível enviar o email.");
            }

            return retorno;
        }


        public bool SendEmailComAnexo(string destinatario, string emailAssunto, string emailCorpo, Bitmap anexo)
        {
            var retorno = false;

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailSettings.EmailUsername));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = emailAssunto;

                // Converte o Bitmap para um stream
                using var ms = new MemoryStream();
                anexo.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                // Cria o corpo do email com o anexo
                var builder = new BodyBuilder();
                builder.HtmlBody = emailCorpo;

                // Anexa a imagem
                var imageAttachment = builder.Attachments.Add("QrCode.png", ms.ToArray(), new ContentType("image", "png"));

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettings.EmailHost, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.EmailUsername, _emailSettings.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);

                retorno = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível enviar o email.", ex);
            }

            return retorno;
        }
    }
}
