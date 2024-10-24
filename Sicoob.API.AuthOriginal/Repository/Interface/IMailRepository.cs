using System.Drawing;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IMailRepository
    {
        bool SendEmail(string destinatario, string emailAssunto, string emailCorpo);
        bool SendEmailComAnexo(string destinatario, string emailAssunto, string emailCorpo, Bitmap anexo);
    }
}
