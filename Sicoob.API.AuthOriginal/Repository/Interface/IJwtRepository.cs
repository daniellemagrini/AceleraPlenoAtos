using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IJwtRepository
    {
        Task<UsuarioSistema> AuthenticateAsync(string login, string senha);
        Task<string> VerifyOtpAndGenerateTokenAsync(string login, string otp);
    }
}
