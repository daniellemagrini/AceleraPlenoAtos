using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acelera.API.AuthOriginal.Model
{
    [Table("03t_GRUPOACESSO")]
    public class GrupoAcesso
    {
        [Display(Name = "ID")]
        [Key]
        public int IDGRUPOACESSO { get; set; }

        [Display(Name = "GRUPO DE ACESSO")]
        public string DESCGRUPOACESSO { get; set; }

        [Display(Name = "CRIADO POR")]
        public int CODCRIADOPOR { get; set; }

        [Display(Name = "DATA CRIACAO")]
        public DateTime DATAHORACRIACAO { get; set; }

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
