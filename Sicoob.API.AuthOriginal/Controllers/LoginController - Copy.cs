using Microsoft.AspNetCore.Mvc;
using OtpNet;
using Sicoob.API.AuthOriginal.Business;
using Sicoob.API.AuthOriginal.DTO;
using Sicoob.API.AuthOriginal.Model;
using Sicoob.API.AuthOriginal.Repository.Interface;

namespace Sicoob.API.AuthOriginal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly LoginBusiness _loginBusiness;
        private readonly IOTPGenerateRepository _otpGenerateRepository;
  
        public AuthController(IUsuarioRepository usuarioRepository, ILoginRepository loginRepository, LoginBusiness loginBusiness,
            IOTPGenerateRepository oTPGenerateRepository)
        {
            _usuarioRepository = usuarioRepository;
            _loginRepository = loginRepository;
            _loginBusiness = loginBusiness;
            _otpGenerateRepository = oTPGenerateRepository;
        }

        [HttpGet("GetAllUsersLogin")]
        public async Task<List<UsuarioSistema>> GetAllUsersLogin()
        {
            var resposta = await _loginRepository.GetAllUsersLogin();
            return resposta;
        }

        [HttpGet("GetAllUsersLoginByID")]
        public async Task<IActionResult> GetAllUsersLoginByID(string id)
        {
            var resposta = await _loginRepository.GetAllUsersLoginByID(id);
            return Ok(resposta);
        }

        [HttpGet("VerificaLoginExistente")]
        public bool VerificaLoginExistente(string login)
        {
            var resposta = _loginRepository.VerificaLoginExistente(login);
            return resposta;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UsuarioSistemaDTO login)
        {
            var resposta = _loginRepository.Login(login);
            if (resposta) return Ok(resposta);
            return BadRequest("Usuário e/ou Senha Incorretos!");
        }

        [HttpPatch("AtualizarSenha")]
        public async Task<IActionResult> AtualizarSenha([FromBody] UsuarioSistemaDTO user)
        {
            var resposta = await _loginRepository.AtualizarSenha(user);
            if (resposta != null) return Ok(new { message = "Senha Atualizada com sucesso!" });
                return BadRequest(new { message = "Não foi possível atualizar a senha. Usuário não existente ou senha inválida." });  
        }

        [HttpPost("EsqueciMinhaSenhaMail")]
        public IActionResult EsqueciMinhaSenhaMail([FromBody] string email)
        {
            var usuario = _loginBusiness.EsqueciMinhaSenhaMail(email);
            if (usuario) return Ok(new { message = "Email enviado para redefinição de senha." });
            return BadRequest(new { message = "Email não existente na base de dados ou não foi possível enviar o email no momento." });
        }

        [HttpPost("VerificaCodAut")]
        public IActionResult VerificaCodAut([FromBody] CodVerificacaoDTO codVerificacao)
        {
            // Obter o usuário a partir do login armazenado na sessão
            var secretKey = _otpGenerateRepository.GetSecretKeyByLogin(codVerificacao.Login);

            if (secretKey == null) return BadRequest(new { message = "Usuário não encontrado." });
       
            bool isValid = _otpGenerateRepository.VerificaSecretKey(secretKey, codVerificacao.Code);

            if (isValid) return Ok(new { message ="Código válido!"}); return BadRequest(new { message = "Código inválido." });
        }

        [HttpGet("GenerateManualTotp")]
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

        [HttpGet("GetUserProfiles/{login}")]
        public async Task<IActionResult> GetUserProfiles(string login)
        {
            var perfis = await _loginRepository.GetPerfil(login);
            if (perfis != null || perfis.Any()) return Ok(perfis);
            return BadRequest(new { message = "Usuário não encontrado ou não possui perfis de acesso." });          
        }

    }
}