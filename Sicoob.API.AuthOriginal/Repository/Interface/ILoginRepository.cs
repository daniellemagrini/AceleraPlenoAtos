using Microsoft.AspNetCore.Mvc;
using Acelera.API.AuthOriginal.DTO;
using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<List<UsuarioSistema>> GetAllUsersLogin();
        Task<UsuarioSistema> GetUsersLoginByID(int id);
        Task<UsuarioSistema> GetUsersByLogin(string login);
        Task<string> GetLoginByEmail(string email);
        bool VerificaLoginExistente(string login);
        Task<bool> VerificaLoginInativo(string login);
        bool PrimeiroAcessoMail(string email);
        Task<ActionResult<UsuarioSistema>> AtualizarSenha(UsuarioSistemaDTO user);
        bool EsqueciMinhaSenhaMail(string email);
        bool Login(UsuarioSistemaDTO login);
    }
}
