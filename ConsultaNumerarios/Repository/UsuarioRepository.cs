using ConsultaNumerarios.Api.Models;
using ConsultaNumerarios.Data;
using ConsultaNumerarios.Interfaces;

namespace ConsultaNumerarios.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _context;
        public UsuarioRepository(DataContext context)
        {
            _context = context;
        }

        public UsuarioModel GetUsuarioPorId(string idUsuario)
        {
            return _context.Usuario.Where(us => us.IdUsuario == idUsuario).FirstOrDefault(); ;
        }
    }
}
