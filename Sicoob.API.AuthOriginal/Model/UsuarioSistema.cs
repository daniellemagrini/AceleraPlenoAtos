using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acelera.API.AuthOriginal.Model
{
    [Table("02t_USUARIOSISTEMA")]
    public class UsuarioSistema
    {
        [Display(Name = "ID")]
        [Key]
        public int IDUSUARIOSISTEMA { get; set; }

        [Display(Name = "ID")]
        public string IDUSUARIO { get; set; }

        [Display(Name = "Login")]
        public string LOGIN { get; set; }

        [Display(Name = "SENHA SALT")]
        public byte[]? SENHASALT { get; set; }

        [Display(Name = "SENHA HASH")]
        public byte[]? SENHAHASH { get; set; }

        [Display(Name = "SECRETKEY")]
        public string SECRETKEY { get; set; }

        [Display(Name = "PRIMEIRO LOGIN")]
        public int BOLPRIMEIROLOGIN { get; set; }

        [Display(Name = "ULTIMO LOGIN")]
        public DateTime? ULTIMOLOGIN { get; set; }

        [Display(Name = "CRIADO POR")]
        public int CODCRIADOPOR { get; set; }

        [Display(Name = "DATA CRIACAO")]
        public DateTime? DATAHORACRIACAO { get; set; }

        [Display(Name = "ALTERADO POR")]
        public int? CODALTERADOPOR { get; set; }

        [Display(Name = "DATA ALTERACAO")]
        public DateTime? DATAHORAALTERACAO { get; set; }

        [Display(Name = "INATIVO POR")]
        public int? CODINATIVOPOR { get; set; }

        [Display(Name = "DATA INATIVO")]
        public DateTime? DATAHORAINATIVO { get; set; }
    }
}
