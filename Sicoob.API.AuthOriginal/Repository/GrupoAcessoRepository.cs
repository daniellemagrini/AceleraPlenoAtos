using Microsoft.EntityFrameworkCore;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Repository
{
    public class GrupoAcessoRepository : IGrupoAcessoRepository
    {
        private readonly AppDbContext _context;       

        public GrupoAcessoRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método que busca todos os Grupos de acesso cadastrados no sistema.
        /// </summary> 
        public async Task<List<GrupoAcesso>> GetAllGrupoAcesso()
        {
            try
            {
                return await _context.GrupoAcesso.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de perfis. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca os perfis de um usuário.
        /// </summary>
        public async Task<List<string>> GetPerfil(string login)
        {
            List<string> listaPerfil = new List<string>();

            try
            {
                var user = await _context.Login.FirstOrDefaultAsync(u => u.LOGIN == login);

                if (user != null)
                {
                    listaPerfil = await (from usga in _context.UsuarioGrupoAcesso
                                         join ga in _context.GrupoAcesso on usga.IDGRUPOACESSO equals ga.IDGRUPOACESSO
                                         where usga.IDUSUARIOSISTEMA == user.IDUSUARIOSISTEMA
                                         select ga.DESCGRUPOACESSO).ToListAsync();
                }

                return listaPerfil;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível listar os perfis deste login. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca as descrições dos perfis pelo seu ID.
        /// </summary>
        public async Task<GrupoAcesso> GetPerfilByID(int id)
        {
            try
            {
                return await _context.GrupoAcesso.FirstOrDefaultAsync(u => u.IDGRUPOACESSO == id);               
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível listar os perfis deste login. Tente novamente mais tarde!");
            }
        }
    }
}
