using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UserController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<Usuario>> GetAllUsers()
        {
            var resposta = await _usuarioRepository.GetAllUsers();
            return resposta;
        }

        [HttpGet("GetUserByID")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            var resposta = await _usuarioRepository.GetUserByID(id);
            if (resposta != null) return Ok(resposta);
            return NotFound(new { message = "Usuário não encontrado." });
        } 

        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var resposta = await _usuarioRepository.GetUserByEmail(email);
            if (resposta != null) return Ok(resposta);
            return NotFound(new { message = "Usuário não encontrado." });
        }

        [HttpGet("GetUserByNome")]
        public async Task<IActionResult> GetUserByNome(string nome)
        {
            var resposta = await _usuarioRepository.GetUserByNome(nome);
            if (resposta != null) return Ok(resposta);
            return NotFound(new { message = "Usuário não encontrado." });
        }

        [HttpGet("VerificaUsuarioExistente")]
        public bool VerificaUsuarioExistente(string cpf)
        {
            var resposta = _usuarioRepository.VerificaUsuarioExistente(cpf);
            return resposta;
        }

        [HttpPost("CadastroUsuario")]
        public async Task<IActionResult> CadastroUsuario([FromBody] UsuarioDTO user, [FromQuery]  IList<int> listaGrupoAcesso, int loginCriador)
        {
            try
            {
                var resposta = await _usuarioRepository.CadastroUsuario(user, listaGrupoAcesso, loginCriador);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario(UsuarioPerfilDTO user, int loginAlteradoPor)
        {
            try
            {
                var resposta = await _usuarioRepository.UpdateUsuario(user, user.Perfis, loginAlteradoPor);
                return Ok(new { message = "Usuário atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("InativarUsuario")]
        public async Task<IActionResult> InativarUsuario(UsuarioPerfilDTO user, int loginInativadoPor)
        {
            try
            {
                var resposta = await _usuarioRepository.InativarUsuario(user, user.Perfis, loginInativadoPor);
                return Ok(new { message = "Usuário inativado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}