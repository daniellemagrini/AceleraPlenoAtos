using Acelera.API.ParamLog.DTO;
using Acelera.API.ParamLog.Model;
using Microsoft.AspNetCore.Mvc;

namespace Acelera.API.ParamLog.Repository.Interface
{
    public interface IParametrizacaoRepository
    {
        Task<List<Parametrizacao>> GetAllParametrizacao();
        Task<ActionResult<Parametrizacao>> GetParametrizacaoByID(int id);
        Task<bool> UpdateParametrizacao(ParametrizacaoDTO paramDTO, int loginAlteradoPor);
        Task<bool> InativarParametrizacao(int id, int loginInativoPor);
        Task<bool> VerificaParametrizacaoInativa(int idParam);
    }
}
