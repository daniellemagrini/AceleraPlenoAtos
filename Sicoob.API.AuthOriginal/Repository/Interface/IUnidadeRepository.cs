using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IUnidadeRepository
    {
        Task<List<Unidade>> GetAllUnidades();
        Task<string> GetUnidadeByLogin(string login);
    }
}
