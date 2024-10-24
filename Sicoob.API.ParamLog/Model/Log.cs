using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acelera.API.ParamLog.Model
{
    [Table("13t_LOG")]
    public class Log
    {
        [Display(Name = "ID")]
        [Key]
        public int IDLOG { get; set; }

        [Display(Name = "MENSAGEM")]
        public string DESCLOG { get; set; }

        [Display(Name = "STATUS")]
        public string STATUS { get; set; }

        [Display(Name = "DATA/HORA")]
        public DateTime DATAHORALOG { get; set; }
    }
}
