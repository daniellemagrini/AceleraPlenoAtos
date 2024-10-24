using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly IGrupoAcessoRepository _grupoAcessoRepository;

        public PerfilController(IGrupoAcessoRepository grupoAcessoRepository)
        {      
            _grupoAcessoRepository = grupoAcessoRepository;
        }

        [HttpGet("GetAllGrupoAcesso")]
        public async Task<List<GrupoAcesso>> GetAllGrupoAcesso()
        {
            var resposta = await _grupoAcessoRepository.GetAllGrupoAcesso();
            return resposta;
        }

        [HttpGet("GetUserProfiles/{login}")]
        public async Task<IActionResult> GetUserProfiles(string login)
        {
            var perfis = await _grupoAcessoRepository.GetPerfil(login);
            if (perfis != null || perfis.Any()) return Ok(perfis);
            return BadRequest(new { message = "Usuário não encontrado ou não possui perfis de acesso." });
        }

        [HttpGet("GetPerfilByID")]
        public async Task<IActionResult> GetPerfilByID(int id)
        {
            var resposta = await _grupoAcessoRepository.GetPerfilByID(id);
            if (resposta != null) return Ok(resposta);
            return BadRequest("Perfil não encontrado!");
        }
    }
}