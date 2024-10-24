using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeRepository _unidadeRepository;

        public UnidadeController(IUnidadeRepository unidadeRepository)
        {
            _unidadeRepository = unidadeRepository;
        }

        [HttpGet("GetAllUnidades")]
        public async Task<List<Unidade>> GetAllUnidades()
        {
            var resposta = await _unidadeRepository.GetAllUnidades();
            return resposta;
        }

        [HttpGet("GetUnidadeByLogin/{login}")]
        public async Task<IActionResult> GetUnidadeByLogin(string login)
        {
            var unidade = await _unidadeRepository.GetUnidadeByLogin(login);
            if (unidade != null) return Ok(new { unidade });
            return BadRequest(new { message = "Usuário não encontrado ou não possui unidade de acesso." });
        }
    }
}