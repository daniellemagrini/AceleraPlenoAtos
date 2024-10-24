namespace Acelera.API.AuthOriginal.Helpers
{
    public interface ISenhaHash
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificaSenhaHash(string id, string senha, byte[] senhaHash, byte[] senhaSalt);
    }
}
