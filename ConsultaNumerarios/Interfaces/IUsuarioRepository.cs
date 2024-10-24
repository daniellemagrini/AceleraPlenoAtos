using ConsultaNumerarios.Api.Models;

namespace ConsultaNumerarios.Interfaces
{
    public interface IUsuarioRepository
    {
        UsuarioModel GetUsuarioPorId(string idUsuario);
    }
}
