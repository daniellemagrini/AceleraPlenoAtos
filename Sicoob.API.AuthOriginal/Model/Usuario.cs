using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Acelera.API.AuthOriginal.Business;

namespace Acelera.API.AuthOriginal.Model
{
    [Table("11t_USUARIO")]
    public class Usuario
    {
        [Display(Name = "ID")]
        [Key]
        public string IDUSUARIO { get; set; }

        [Display(Name = "ID UNIDADE INST")]
        public string IDUNIDADEINST { get; set; }

        [Display(Name = "ID INSTITUICAO")]
        public int IDINSTITUICAO { get; set; }

        [Display(Name = "ALTERACAO")]
        public int? NUMCHECKALTERACAO { get; set; }

        [Display(Name = "ID INSTITUICAO USUARIO")]
        public int IDINSTITUICAOUSUARIO { get; set; }

        [Display(Name = "USUARIO")]
        public string DESCNOMEUSUARIO { get; set; }

        [Display(Name = "EMAIL")]
        public string DESCEMAIL { get; set; }

        [Display(Name = "USUARIO HABILITADO")]
        public int? BOLHABILITADOUSUARIO { get; set; }

        [Display(Name = "STATUS USUARIO")]
        public string? DESCSTATUSUSUARIO { get; set; }

        [Display(Name = "MAQUINA")]
        public int? BOLVERIFICANOMEMAQUINA { get; set; }

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
