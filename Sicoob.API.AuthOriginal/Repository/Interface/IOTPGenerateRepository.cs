using System.Drawing;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IOTPGenerateRepository
    {
        string GerarSecretKey(string email);
        string GerarURISecretKey(string email, string secretKey);
        Bitmap CriarQRCode(string uriString);
        bool VerificaSecretKey(string secretKey, string codigo);
        string GetSecretKeyByLogin(string login);

    }
}
