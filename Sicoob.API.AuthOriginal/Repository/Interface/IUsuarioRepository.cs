using Microsoft.AspNetCore.Mvc;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllUsers();
        Task<ActionResult<UsuarioPerfilDTO>> GetUserByID(string id);
        Task<ActionResult<UsuarioPerfilDTO>> GetUserByEmail(string email);
        Task<ActionResult<UsuarioPerfilDTO>> GetUserByNome(string nome);
        bool VerificaUsuarioExistente(string cpf);
        Task<Usuario> CadastroUsuario(UsuarioDTO user, IList<int> listaGrupoAcesso, int loginCriador);
        Task<bool> UpdateUsuario(UsuarioPerfilDTO usuarioDto, IList<string> listaGrupoAcesso, int loginAlteradoPor);
        Task<bool> InativarUsuario(UsuarioPerfilDTO user, IList<string> listaGrupoAcesso, int loginInativadoPor);
    }
}
