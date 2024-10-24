using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Acelera.API.ParamLog.DTO;
using Acelera.API.ParamLog.Model;
using Acelera.API.ParamLog.Repository.Interface;

namespace Acelera.API.ParamLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParamController : ControllerBase
    {
        private readonly IParametrizacaoRepository _parametrizacaoRepository;

        public ParamController(IParametrizacaoRepository parametrizacaoRepository)
        {
            _parametrizacaoRepository = parametrizacaoRepository;
        }

        [HttpGet("GetAllParametrizacao")]
        public async Task<List<Parametrizacao>> GetAllParametrizacao()
        {
            var resposta = await _parametrizacaoRepository.GetAllParametrizacao();
            return resposta;
        }

        [HttpGet("GetParametrizacaoByID")]
        public async Task<IActionResult> GetParametrizacaoByID(int id)
        {
            try
            {
                var resposta = await _parametrizacaoRepository.GetParametrizacaoByID(id);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateParametrizacao")]
        public async Task<IActionResult> UpdateParametrizacao(ParametrizacaoDTO paramDTO, int loginAlteradoPor)
        {
            try
            {
                var resposta = await _parametrizacaoRepository.UpdateParametrizacao(paramDTO, loginAlteradoPor);
                return Ok(new { message = "Parametrização atualizada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("InativarParametrizacao")]
        public async Task<IActionResult> InativarParametrizacao(int id, int loginInativoPor)
        {
            try
            {
                var resposta = await _parametrizacaoRepository.InativarParametrizacao(id, loginInativoPor);
                return Ok(new { message = "Parametrização inativada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("VerificaParametrizacaoInativa")]
        public Task<bool> VerificaParametrizacaoInativa(int idParam)
        {
            var resposta = _parametrizacaoRepository.VerificaParametrizacaoInativa(idParam);
            return resposta;
        }
    }
}
