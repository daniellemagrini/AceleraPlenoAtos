using System.ComponentModel.DataAnnotations;

namespace Acelera.API.AuthOriginal.DTO
{
    public class UsuarioSistemaDTO
    {
        [Required(ErrorMessage = "O campo login é obrigatório!")]
        public string LOGIN { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        public string SENHA { get; set; }
    }
}
