using System.ComponentModel.DataAnnotations;

namespace Acelera.API.AuthOriginal.DTO
{
    public class UsuarioPerfilDTO
    {
        [Required(ErrorMessage = "O campo ID é obrigatório!")]
        public string IDUSUARIO { get; set; }

        [Required(ErrorMessage = "O campo Unidade de Instituição é obrigatório!")]
        public string IDUNIDADEINST { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        public string DESCNOMEUSUARIO { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório!")]
        public string DESCEMAIL { get; set; }

        [Required(ErrorMessage = "O campo Perfil é obrigatório!")]
        public List<string> Perfis { get; set; }

        public int? CODINATIVOPOR { get; set; }
    }
}
