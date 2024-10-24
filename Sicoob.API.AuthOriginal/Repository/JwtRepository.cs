using Acelera.API.AuthOriginal.Repository.Interface;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Acelera.API.AuthOriginal.Helpers;

namespace Acelera.API.AuthOriginal.Repository
{
    public class JwtRepository : IJwtRepository
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IOTPGenerateRepository _oTPGenerateRepository;
        private readonly ISenhaHash _senhaHash;

        public JwtRepository(AppDbContext context, JwtSettings jwtSettings, IOTPGenerateRepository oTPGenerateRepository, ISenhaHash senhaHash)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _oTPGenerateRepository = oTPGenerateRepository;
            _senhaHash = senhaHash;
        }

        public async Task<UsuarioSistema> AuthenticateAsync(string login, string senha)
        {
            var userSistema = await _context.Login.FirstOrDefaultAsync(u => u.LOGIN == login);

            if (userSistema == null || !_senhaHash.VerificaSenhaHash(userSistema.IDUSUARIO, senha, userSistema.SENHAHASH, userSistema.SENHASALT))
            {
                return null;
            }

            return userSistema;
        }

        public async Task<string> VerifyOtpAndGenerateTokenAsync(string login, string otp)
        {
            var userSistema = await _context.Login.FirstOrDefaultAsync(u => u.LOGIN == login);
            if (userSistema == null) return null;
            
            var secretKey = _oTPGenerateRepository.GetSecretKeyByLogin(userSistema.IDUSUARIO);
            var validOtp = _oTPGenerateRepository.VerificaSecretKey(secretKey, otp);
            if (!validOtp) return null;         

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.IDUSUARIO == userSistema.IDUSUARIO);
            var perfis = await _context.UsuarioGrupoAcesso
                .Where(uga => uga.IDUSUARIOSISTEMA == userSistema.IDUSUARIOSISTEMA)
                .Join(_context.GrupoAcesso,
                      uga => uga.IDGRUPOACESSO,
                      ga => ga.IDGRUPOACESSO,
                      (uga, ga) => ga.DESCGRUPOACESSO)
                .ToListAsync();

            return GenerateToken(userSistema, user, perfis);
        }

        private string GenerateToken(UsuarioSistema userSistema, Usuario user, List<string> perfis)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userSistema.IDUSUARIOSISTEMA.ToString()),
                new Claim(ClaimTypes.Name, user.DESCNOMEUSUARIO)
            };

            // Adiciona os perfis como claims
            claims.AddRange(perfis.Select(perfil => new Claim(ClaimTypes.Role, perfil)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
