using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IGrupoAcessoRepository
    {
        Task<List<GrupoAcesso>> GetAllGrupoAcesso();
        Task<List<string>> GetPerfil(string login);
        Task<GrupoAcesso> GetPerfilByID(int id);
    }
}
