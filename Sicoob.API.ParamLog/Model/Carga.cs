using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acelera.API.ParamLog.Model
{
    [Table("01t_CARGA")]
    public class Carga
    {
        [Display(Name = "ID")]
        [Key]
        public int IDCARGA { get; set; }

        [Display(Name = "ID LOGIN")]
        public string IDUSUARIOSISTEMA { get; set; }

        [Display(Name = "ARQUIVO")]
        public string DESCARQUIVO { get; set; }

        [Display(Name = "TIPO")]
        public string TIPOARQUIVO { get; set; }

        [Display(Name = "CAMINHO")]
        public string CAMINHOCARGAUPLOAD { get; set; }

        [Display(Name = "STATUS UPLOAD")]
        public string ESTADOUPLOAD { get; set; }

        [Display(Name = "STATUS CARGA")]
        public string ESTADOCARGA { get; set; }

        [Display(Name = "MENSAGEM")]
        public string MENSAGEMERRO { get; set; }

        [Display(Name = "DATA HORA UPLOAD")]
        public string DATAHORAUPLOAD { get; set; }

        [Display(Name = "DATA HORA CARGA")]
        public string DATAHORACARGA { get; set; }
    }
}
