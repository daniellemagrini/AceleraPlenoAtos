using Microsoft.EntityFrameworkCore;
using Acelera.API.AuthOriginal.Data;
using Acelera.API.AuthOriginal.Model;
using Acelera.API.AuthOriginal.Repository.Interface;

namespace Acelera.API.AuthOriginal.Repository
{
    public class UnidadeRepository : IUnidadeRepository
    {
        private readonly AppDbContext _context;

        public UnidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método que busca todos as unidades cadastradas no sistema.
        /// </summary> 
        public async Task<List<Unidade>> GetAllUnidades()
        {
            try
            {
                return await _context.Unidades.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de unidades. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca a unidade de um usuário.
        /// </summary>
        public async Task<string> GetUnidadeByLogin(string login)
        {
            string unidade = string.Empty;

            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.IDUSUARIO == login);

                if (user != null)
                {
                    unidade = await _context.Unidades
                                    .Where(u => u.IDUNIDADEINST == user.IDUNIDADEINST)
                                    .Select(u => u.NOMEUNIDADE)
                                    .FirstOrDefaultAsync();
                }

                return unidade;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar a unidade deste login. Tente novamente mais tarde!");
            }
        }
    }
}
