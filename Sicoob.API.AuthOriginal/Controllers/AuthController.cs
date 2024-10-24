using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using Acelera.API.AuthOriginal.Business;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Helpers;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly LoginBusiness _loginBusiness;
        private readonly IOTPGenerateRepository _otpGenerateRepository;
        private readonly IJwtRepository _jwtRepository;
        private readonly ISenhaHash _senhaHash;
        private readonly AppDbContext _context;

        public AuthController(IUsuarioRepository usuarioRepository, ILoginRepository loginRepository, LoginBusiness loginBusiness,
            IOTPGenerateRepository oTPGenerateRepository, IJwtRepository jwtRepository, ISenhaHash senhaHash, AppDbContext context)
        {
            _usuarioRepository = usuarioRepository;
            _loginRepository = loginRepository;
            _loginBusiness = loginBusiness;
            _otpGenerateRepository = oTPGenerateRepository;
            _jwtRepository = jwtRepository;
            _senhaHash = senhaHash;
            _context = context;
        }

        [HttpGet("GetAllUsersLogin")]
        public async Task<List<UsuarioSistema>> GetAllUsersLogin()
        {
            var resposta = await _loginRepository.GetAllUsersLogin();
            return resposta;
        }

        [HttpGet("GetUsersLoginByID")]
        public async Task<IActionResult> GetUsersLoginByID(int id)
        {
            var resposta = await _loginRepository.GetUsersLoginByID(id);
            if (resposta != null) return Ok(resposta);
            return BadRequest("Usuário não encontrado!");
        }

        [HttpGet("GetUsersByLogin")]
        public async Task<IActionResult> GetUsersByLogin(string login)
        {
            var resposta = await _loginRepository.GetUsersByLogin(login);
            if (resposta != null) return Ok(resposta);
            return BadRequest("Usuário não encontrado!");
        }

        [HttpGet("GetLoginByEmail")]
        public async Task<IActionResult> GetLoginByEmail(string email)
        {
            var resposta = await _loginRepository.GetLoginByEmail(email);
            if (resposta != null) return Ok(resposta);
            return BadRequest("Usuário não encontrado!");
        }

        [HttpGet("VerificaLoginExistente")]
        public bool VerificaLoginExistente(string login)
        {
            var resposta = _loginRepository.VerificaLoginExistente(login);
            return resposta;
        }

        [HttpGet("VerificaLoginInativo")]
        public Task<bool> VerificaLoginInativo(string login)
        {
            var resposta = _loginRepository.VerificaLoginInativo(login);
            return resposta;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioSistemaDTO login)
        {
            var userSistema = await _jwtRepository.AuthenticateAsync(login.LOGIN, login.SENHA);
            if (userSistema != null) return Ok(new { message = "Credenciais verificadas, por favor insira o código OTP.", tempLogin = login.LOGIN });
                    return BadRequest("Usuário e/ou Senha Incorretos!");  
        }

        [HttpPatch("AtualizarSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> AtualizarSenha([FromBody] UsuarioSistemaDTO user)
        {
            var resposta = await _loginRepository.AtualizarSenha(user);
            if (resposta != null) return Ok(new { message = "Senha Atualizada com sucesso!" });
                return BadRequest(new { message = "Não foi possível atualizar a senha. Usuário não existente ou senha inválida." });  
        }

        [HttpPost("EsqueciMinhaSenhaMail")]
        [AllowAnonymous]
        public IActionResult EsqueciMinhaSenhaMail([FromBody] string email)
        {
            var usuario = _loginBusiness.EsqueciMinhaSenhaMail(email);
            if (usuario) return Ok(new { message = "Email enviado para redefinição de senha." });
            return BadRequest(new { message = "Email não existente na base de dados ou não foi possível enviar o email no momento." });
        }

        [HttpPost("VerificaCodAut")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificaCodAut([FromBody] CodVerificacaoDTO codVerificacao)
        {
            var token = await _jwtRepository.VerifyOtpAndGenerateTokenAsync(codVerificacao.Login, codVerificacao.Code);
            if (token != null) return Ok(new { Token = token });
                return BadRequest(new { message = "Código de verificação inválido." });
        }

        [HttpGet("GenerateManualTotp")]
        [AllowAnonymous]
        public IActionResult GenerateManualTotp(string secretKey)
        {
            try
            {
                var totp = new Totp(Base32Encoding.ToBytes(secretKey));
                var generatedCode = totp.ComputeTotp(); // Gera o código TOTP atual
                return Ok(new { code = generatedCode });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar TOTP: {ex.Message}");
                return StatusCode(500, new { message = $"Erro ao gerar TOTP: {ex.Message}" });
            }
        }
    }
}