using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acelera.API.ParamLog.Model
{
    [Table("00t_PARAMETRIZACAO")]
    public class Parametrizacao
    {
        [Display(Name = "ID")]
        [Key]
        public int IDPARAMETRIZACAO { get; set; }

        [Display(Name = "TIPO CARGA")]
        public string? TIPOCARGA { get; set; }

        [Display(Name = "CAMINHO CARGA")]
        public string? CAMINHOCARGA { get; set; }

        [Display(Name = "INTERVALO EXECUCAO")]
        public int? INTERVALOEXECUCAO { get; set; }

        [Display(Name = "CRIADO POR")]
        public int CODCRIADOPOR { get; set; }

        [Display(Name = "DATA HORA CRIACAO")]
        public DateTime DATAHORACRIACAO { get; set; }

        [Display(Name = "ALTERADO POR")]
        public int? CODALTERADOPOR { get; set; }

        [Display(Name = "DATA HORA CRIACAO")]
        public DateTime? DATAHORAALTERACAO { get; set; }

        [Display(Name = "INATIVO POR")]
        public int? CODINATIVOPOR { get; set; }

        [Display(Name = "DATA HORA INATIVO")]
        public DateTime? DATAHORAINATIVO { get; set; }
    }
}
