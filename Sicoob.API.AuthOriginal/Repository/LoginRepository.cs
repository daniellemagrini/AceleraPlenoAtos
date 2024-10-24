using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Helpers;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;
        private readonly ISenhaHash _senhaHash;
        public static UsuarioSistema novoLogin = new UsuarioSistema();

        public LoginRepository(AppDbContext context, ISenhaHash senhaHash)
        {
            _context = context;
            _senhaHash = senhaHash;
    }

        /// <summary>
        /// Método que busca todos os usuários com login cadastrados no sistema.
        /// </summary> 
        public async Task<List<UsuarioSistema>> GetAllUsersLogin()
        {
            try
            {
                return await _context.Login.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de logins. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca os usuários com login cadastrados no sistema pelo ID de usuário.
        /// </summary> 
        public async Task<UsuarioSistema> GetUsersLoginByID(int id)
        {
            try
            {
                return await _context.Login.SingleOrDefaultAsync(x => x.IDUSUARIOSISTEMA == id);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar o login desse ID. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca os usuários no sistema pelo login de usuário.
        /// </summary> 
        public async Task<UsuarioSistema> GetUsersByLogin(string login)
        {
            try
            {
                return await _context.Login.SingleOrDefaultAsync(x => x.LOGIN == login);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar o ID desse login. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca os usuários no sistema pelo login de usuário.
        /// </summary> 
        public async Task<string> GetLoginByEmail(string email)
        {
            try
            {
                var user = await _context.Usuarios.SingleOrDefaultAsync(x => x.DESCEMAIL == email);
                return user.IDUSUARIO;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar o ID desse email. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para verificar se já existe um cadastro no sistema com esse login.
        /// </summary>
        public bool VerificaLoginExistente(string login)
        {
            try
            {
                if (_context.Login.Any(x => x.LOGIN == login))
                    return true; return false;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se o login já exite no sistema. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para verificar se o login está inativo.
        /// </summary>
        public async Task<bool> VerificaLoginInativo(string login)
        {
            var resposta = false;

            try
            {
                var usuario = await _context.Login.SingleOrDefaultAsync(x => x.LOGIN == login);
                if(usuario != null)
                {
                    if (usuario.CODINATIVOPOR != null)
                    {
                        resposta = true;
                    }
                }
                else
                {
                    throw new Exception("Usuário não encontrado!");
                }

                return resposta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método de envio de email para definição de senha no primeiro acesso.
        /// </summary>
        public bool PrimeiroAcessoMail(string email)
        {
            try
            {
                var usuario = _context.Usuarios.SingleOrDefaultAsync(x => x.DESCEMAIL == email);

                if(usuario != null) return true; return false;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível enviar o email de primeiro acesso. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para cadastrar senha de login.
        /// </summary>
        public async Task<ActionResult<UsuarioSistema>> AtualizarSenha(UsuarioSistemaDTO user)
        {
            UsuarioSistema usuario = null;

            try
            {
                //var idLogin = await _context.Login
                //    .Where(x => x.IDUSUARIO == login)
                //    .Select(x => x.IDUSUARIO)
                //    .SingleOrDefaultAsync();

                usuario = _context.Login.SingleOrDefault(x =>
                     x.LOGIN == user.LOGIN);

                if (usuario != null)
                {
                    _senhaHash.CriarSenhaHash(user.SENHA, out byte[] senhaHash, out byte[] senhaSalt);

                    //await _context.Login
                    //             .Where(u => u.IDUSUARIO == login)
                    //             .ExecuteUpdateAsync(p => p
                    //                 .SetProperty(pr => pr.SENHASALT, senhaSalt)
                    //                 .SetProperty(pr => pr.SENHAHASH, senhaHash));

                    usuario.SENHAHASH = senhaHash;
                    usuario.SENHASALT = senhaSalt;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Usuário não existente. Favor entrar em contato com sua gerência");
                }

                return usuario;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível atualizar a senha. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para realizar o login.
        /// </summary>
        public bool Login(UsuarioSistemaDTO login)
        {
            UsuarioSistema usuario = null;
            bool resposta = false;

            try
            {
                usuario = _context.Login.SingleOrDefault(x =>
                     x.IDUSUARIO == login.LOGIN);

                if (usuario != null)
                {
                    resposta = _senhaHash.VerificaSenhaHash(login.LOGIN, login.SENHA, usuario.SENHAHASH, usuario.SENHASALT);        
                }

                return resposta;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível realizar o login. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método Esqueci minha senha.
        /// </summary>
        public async Task<ActionResult<string>> EsqueciMinhaSenha(string login, string novaSenha)
        {
            try
            {
                var idLogin = await _context.Login
                    .Where(x => x.IDUSUARIO == login)
                    .Select(x => x.IDUSUARIO)
                    .SingleOrDefaultAsync();

                if (idLogin != null)
                {
                    var senhaSalt = await _context.Login
                    .Where(x => x.IDUSUARIO == login)
                    .Select(x => x.SENHASALT)
                    .SingleOrDefaultAsync();

                    var senhaHash = await _context.Login
                    .Where(x => x.IDUSUARIO == login)
                    .Select(x => x.SENHAHASH)
                    .SingleOrDefaultAsync();


                    _senhaHash.CriarSenhaHash(novaSenha, out byte[] novaSenhaHash, out byte[] novaSenhaSalt);

                    await _context.Login
                                 .Where(u => u.IDUSUARIO == login)
                                 .ExecuteUpdateAsync(p => p
                                     .SetProperty(pr => pr.SENHASALT, novaSenhaSalt)
                                     .SetProperty(pr => pr.SENHAHASH, novaSenhaHash));

                    await _context.SaveChangesAsync();

                }
                return idLogin;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível alterar a senha. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método de envio de email para Esqueci minha senha.
        /// </summary>
        public bool EsqueciMinhaSenhaMail(string email)
        {
            try
            {
                var usuario = _context.Usuarios.SingleOrDefault(x =>
                     x.DESCEMAIL == email);

                if (usuario != null) return true; return false; 
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar o um login associado a este email. Tente novamente mais tarde!");
            }
        }       
    }
}
