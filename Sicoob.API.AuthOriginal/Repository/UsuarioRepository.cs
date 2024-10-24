using AceleraPlenoProjetoFinal.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acelera.API.AuthOriginal.Business;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Helpers;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISenhaHash _senhaHash;
        private readonly ILoginRepository _loginRepository;
        private readonly LoginBusiness _loginBusiness;
        private readonly IOTPGenerateRepository _oTPGenerateRepository;
        public static Usuario novoUsuario = new Usuario();
        public static UsuarioSistema novoLogin = new UsuarioSistema();
        public static UsuarioSistemaGrupoAcesso novoGrupoAcesso = new UsuarioSistemaGrupoAcesso();
        public static UsuarioPerfilDTO usuarioPerfil = new UsuarioPerfilDTO();

        public UsuarioRepository(AppDbContext context, ILoginRepository loginRepository, IOTPGenerateRepository oTPGenerateRepository,
            LoginBusiness loginBusiness, ISenhaHash senhaHash, IMapper mapper)
        {
            _context = context;
            _loginRepository = loginRepository;
            _oTPGenerateRepository = oTPGenerateRepository;
            _loginBusiness = loginBusiness;
            _senhaHash = senhaHash;
            _mapper = mapper;
        }

        /// <summary>
        /// Método que busca todos os usuários cadastrados no sistema.
        /// </summary>  
        [HttpGet]
        public async Task<List<Usuario>> GetAllUsers()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de usuários. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca um usuário pelo seu ID.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<UsuarioPerfilDTO>> GetUserByID(string id)
        {
            UsuarioPerfilDTO usuarioPerfil = new UsuarioPerfilDTO();

            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(x => x.IDUSUARIO == id);

                if (usuario != null)
                {
                    var perfis = await (from u in _context.Usuarios
                                        join us in _context.Login on u.IDUSUARIO equals us.IDUSUARIO
                                        join usga in _context.UsuarioGrupoAcesso on us.IDUSUARIOSISTEMA equals usga.IDUSUARIOSISTEMA
                                        join ga in _context.GrupoAcesso on usga.IDGRUPOACESSO equals ga.IDGRUPOACESSO
                                        where u.IDUSUARIO == usuario.IDUSUARIO
                                        select ga.DESCGRUPOACESSO).ToListAsync();

                    usuarioPerfil.IDUSUARIO = usuario.IDUSUARIO;
                    usuarioPerfil.IDUNIDADEINST = usuario.IDUNIDADEINST;
                    usuarioPerfil.DESCNOMEUSUARIO = usuario.DESCNOMEUSUARIO;
                    usuarioPerfil.DESCEMAIL = usuario.DESCEMAIL;
                    usuarioPerfil.Perfis = perfis;
                    usuarioPerfil.CODINATIVOPOR = usuario.CODINATIVOPOR;
                }

                else throw new Exception("Usuário não encontrado.");

                return usuarioPerfil;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método que busca um usuário pelo seu e-mail.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<UsuarioPerfilDTO>> GetUserByEmail(string email)
        {
            UsuarioPerfilDTO usuarioPerfil = new UsuarioPerfilDTO();

            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(x => x.DESCEMAIL == email);

                if (usuario != null)
                {
                    var perfis = await (from u in _context.Usuarios
                                        join us in _context.Login on u.IDUSUARIO equals us.IDUSUARIO
                                        join usga in _context.UsuarioGrupoAcesso on us.IDUSUARIOSISTEMA equals usga.IDUSUARIOSISTEMA
                                        join ga in _context.GrupoAcesso on usga.IDGRUPOACESSO equals ga.IDGRUPOACESSO
                                        where u.IDUSUARIO == usuario.IDUSUARIO
                                        select ga.DESCGRUPOACESSO).ToListAsync();

                    usuarioPerfil.IDUSUARIO = usuario.IDUSUARIO;
                    usuarioPerfil.IDUNIDADEINST = usuario.IDUNIDADEINST;
                    usuarioPerfil.DESCNOMEUSUARIO = usuario.DESCNOMEUSUARIO;
                    usuarioPerfil.DESCEMAIL = usuario.DESCEMAIL;
                    usuarioPerfil.Perfis = perfis;
                    usuarioPerfil.CODINATIVOPOR = usuario.CODINATIVOPOR;
                }

                else throw new Exception("Usuário não encontrado.");

                return usuarioPerfil;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Método que busca um usuário pelo seu nome.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<UsuarioPerfilDTO>> GetUserByNome(string nome)
        {
            UsuarioPerfilDTO usuarioPerfil = new UsuarioPerfilDTO();

            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(x => x.DESCNOMEUSUARIO == nome);

                if (usuario != null)
                {
                    var perfis = await (from u in _context.Usuarios
                                        join us in _context.Login on u.IDUSUARIO equals us.IDUSUARIO
                                        join usga in _context.UsuarioGrupoAcesso on us.IDUSUARIOSISTEMA equals usga.IDUSUARIOSISTEMA
                                        join ga in _context.GrupoAcesso on usga.IDGRUPOACESSO equals ga.IDGRUPOACESSO
                                        where u.IDUSUARIO == usuario.IDUSUARIO
                                        select ga.DESCGRUPOACESSO).ToListAsync();

                    usuarioPerfil.IDUSUARIO = usuario.IDUSUARIO;
                    usuarioPerfil.IDUNIDADEINST = usuario.IDUNIDADEINST;
                    usuarioPerfil.DESCNOMEUSUARIO = usuario.DESCNOMEUSUARIO;
                    usuarioPerfil.DESCEMAIL = usuario.DESCEMAIL;
                    usuarioPerfil.Perfis = perfis;
                    usuarioPerfil.CODINATIVOPOR = usuario.CODINATIVOPOR;
                }
                else throw new Exception("Usuário não encontrado.");

                return usuarioPerfil;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método para verificar se já existe um usuário no sistema com esse CPF.
        /// </summary>
        [HttpGet]
        public bool VerificaUsuarioExistente(string id)
        {
            try
            {
                if (_context.Usuarios.Any(x => x.IDUSUARIO == id))
                    return true; return false;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se o usuário já exite no sistema. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para verificar se já existe um usuário no sistema com esse CPF.
        /// </summary>
        [HttpGet]
        public bool VerificaEmailExistente(string email)
        {
            try
            {
                if (_context.Usuarios.Any(x => x.DESCEMAIL == email))
                    return true; return false;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível verificar se o usuário já exite no sistema. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para cadastrar um novo usuário no sistema.
        /// </summary>
        [HttpPost]
        public async Task<Usuario> CadastroUsuario(UsuarioDTO user, IList<int> listaGrupoAcesso, int loginCriador)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (!VerificaUsuarioExistente(user.IDUSUARIO) && !VerificaEmailExistente(user.DESCEMAIL))
                {
                    user.IDUSUARIO = user.IDUSUARIO.ToUpper();
                    user.DESCNOMEUSUARIO = user.DESCNOMEUSUARIO.ToUpper();
                    user.DESCEMAIL = user.DESCEMAIL.ToUpper();

                    var novoUsuario = _mapper.Map<Usuario>(user);
                    novoUsuario.DATAHORACRIACAO = DateTime.Now;
                    await _context.Usuarios.AddAsync(novoUsuario);
                    await _context.SaveChangesAsync();

                    var secretKey = _oTPGenerateRepository.GerarSecretKey(user.DESCEMAIL);

                    var login = _mapper.Map<UsuarioSistema>(user);
                    login.SECRETKEY = secretKey;
                    login.CODCRIADOPOR = loginCriador;
                    login.DATAHORACRIACAO = DateTime.Now;
                    await _context.Login.AddAsync(login);
                    await _context.SaveChangesAsync();

                    novoUsuario.CODCRIADOPOR = loginCriador;
                    _context.Usuarios.Update(novoUsuario);
                    await _context.SaveChangesAsync();

                    foreach (var item in listaGrupoAcesso)
                    {
                        var perfilUser = new UsuarioSistemaGrupoAcesso
                        {
                            IDGRUPOACESSO = item,
                            IDUSUARIOSISTEMA = login.IDUSUARIOSISTEMA,
                            CODCRIADOPOR = loginCriador,
                            DATAHORACRIACAO = DateTime.Now
                        };
                        await _context.UsuarioGrupoAcesso.AddAsync(perfilUser);
                    }
                    await _context.SaveChangesAsync();

                    var uriSecretKey = _oTPGenerateRepository.GerarURISecretKey(user.DESCEMAIL, secretKey);

                    var qrCode = _oTPGenerateRepository.CriarQRCode(uriSecretKey);

                    _loginBusiness.EnviarEmailPrimeiroAcesso(user.DESCNOMEUSUARIO, user.IDUSUARIO, user.DESCEMAIL, qrCode);

                    await transaction.CommitAsync();

                    return novoUsuario;
                }
                else throw new Exception("Usuário e/ou email já existente.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Método para atualizar um usuário no sistema.
        /// </summary>
        public async Task<bool> UpdateUsuario(UsuarioPerfilDTO user, IList<string> listaGrupoAcesso, int loginAlteradoPor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.IDUSUARIO == user.IDUSUARIO);
                if (usuario != null)
                {
                    usuario.IDUNIDADEINST = user.IDUNIDADEINST.ToUpper();
                    usuario.DESCNOMEUSUARIO = user.DESCNOMEUSUARIO.ToUpper();
                    usuario.DESCEMAIL = user.DESCEMAIL.ToUpper();
                    usuario.CODCRIADOPOR = usuario.CODCRIADOPOR;
                    usuario.CODALTERADOPOR = loginAlteradoPor;
                    usuario.DATAHORAALTERACAO = DateTime.Now;

                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    var usuarioSistema = await _context.Login.SingleOrDefaultAsync(us => us.IDUSUARIO == user.IDUSUARIO);

                    if (usuarioSistema != null)
                    {
                        usuarioSistema.CODCRIADOPOR = usuarioSistema.CODCRIADOPOR;
                        usuarioSistema.CODALTERADOPOR = loginAlteradoPor;
                        usuarioSistema.DATAHORAALTERACAO = DateTime.Now;
                        _context.Login.Update(usuarioSistema);
                        await _context.SaveChangesAsync();  

                        var usuarioPerfis = await _context.UsuarioGrupoAcesso.Where(ug => ug.IDUSUARIOSISTEMA == usuarioSistema.IDUSUARIOSISTEMA).ToListAsync();
                        _context.UsuarioGrupoAcesso.RemoveRange(usuarioPerfis);
                        await _context.SaveChangesAsync();

                        var listaPerfil = await _context.GrupoAcesso
                                            .Where(ga => listaGrupoAcesso.Contains(ga.DESCGRUPOACESSO))
                                            .Select(ga => ga.IDGRUPOACESSO)
                                            .ToListAsync();

                        foreach (var item in listaPerfil)
                        {
                            var perfilUser = new UsuarioSistemaGrupoAcesso
                            {
                                IDGRUPOACESSO = item,
                                IDUSUARIOSISTEMA = usuarioSistema.IDUSUARIOSISTEMA,
                                CODCRIADOPOR = usuarioSistema.CODCRIADOPOR,
                                DATAHORACRIACAO = usuario.DATAHORACRIACAO,
                                CODALTERADOPOR = loginAlteradoPor,
                                DATAHORAALTERACAO = DateTime.Now
                            };

                            await _context.UsuarioGrupoAcesso.AddAsync(perfilUser);
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    else return false;    
                }
                else return false;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception("Não foi possível atualizar o usuário no sistema. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método para inativar um usuário no sistema.
        /// </summary>
        public async Task<bool> InativarUsuario(UsuarioPerfilDTO user, IList<string> listaGrupoAcesso, int loginInativadoPor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.IDUSUARIO == user.IDUSUARIO);
                if (usuario != null)
                {
                    usuario.IDUNIDADEINST = user.IDUNIDADEINST.ToUpper();
                    usuario.DESCNOMEUSUARIO = user.DESCNOMEUSUARIO.ToUpper();
                    usuario.DESCEMAIL = user.DESCEMAIL.ToUpper();
                    usuario.CODCRIADOPOR = usuario.CODCRIADOPOR;
                    usuario.DATAHORACRIACAO = usuario.DATAHORACRIACAO;
                    usuario.CODALTERADOPOR = usuario.CODALTERADOPOR;
                    usuario.DATAHORAALTERACAO = usuario.DATAHORAALTERACAO;
                    usuario.CODINATIVOPOR = loginInativadoPor;
                    usuario.DATAHORAINATIVO = DateTime.Now;

                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    var usuarioSistema = await _context.Login.SingleOrDefaultAsync(us => us.IDUSUARIO == user.IDUSUARIO);

                    if (usuarioSistema != null)
                    {
                        usuarioSistema.CODCRIADOPOR = usuario.CODCRIADOPOR;
                        usuarioSistema.DATAHORACRIACAO = usuario.DATAHORACRIACAO;
                        usuarioSistema.CODALTERADOPOR = usuario.CODALTERADOPOR;
                        usuarioSistema.DATAHORAALTERACAO = usuario.DATAHORAALTERACAO;
                        usuarioSistema.CODINATIVOPOR = loginInativadoPor;
                        usuarioSistema.DATAHORAINATIVO = DateTime.Now;
                        _context.Login.Update(usuarioSistema);
                        await _context.SaveChangesAsync();

                        var usuarioPerfis = await _context.UsuarioGrupoAcesso.Where(ug => ug.IDUSUARIOSISTEMA == usuarioSistema.IDUSUARIOSISTEMA).ToListAsync();
                        _context.UsuarioGrupoAcesso.RemoveRange(usuarioPerfis);
                        await _context.SaveChangesAsync();

                        var listaPerfil = await _context.GrupoAcesso
                                            .Where(ga => listaGrupoAcesso.Contains(ga.DESCGRUPOACESSO))
                                            .Select(ga => ga.IDGRUPOACESSO)
                                            .ToListAsync();

                        foreach (var item in listaPerfil)
                        {
                            var perfilUser = new UsuarioSistemaGrupoAcesso
                            {
                                IDGRUPOACESSO = item,
                                IDUSUARIOSISTEMA = usuarioSistema.IDUSUARIOSISTEMA,
                                CODCRIADOPOR = usuarioSistema.CODCRIADOPOR,
                                DATAHORACRIACAO = usuarioSistema.DATAHORACRIACAO,
                                CODALTERADOPOR = usuarioSistema.CODALTERADOPOR,
                                DATAHORAALTERACAO = usuario.DATAHORAALTERACAO,
                                CODINATIVOPOR = loginInativadoPor,
                                DATAHORAINATIVO = DateTime.Now                             
                            };

                            await _context.UsuarioGrupoAcesso.AddAsync(perfilUser);
                        }

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception("Não foi possível inativar o usuário no sistema. Tente novamente mais tarde!");
            }
        }
    }
}
