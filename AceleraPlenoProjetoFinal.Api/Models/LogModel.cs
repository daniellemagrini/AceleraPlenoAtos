using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("13t_LOG")]
public class LogModel
{
    [Key]
    [Column("IDLOG")]
    public int IdLog { get; set; }

    [Column("DESCLOG")]
    public string DescLog { get; set; }

    [Column("STATUS")]
    public string Status { get; set; }

    [Column("DATAHORALOG")]
    public DateTime DataHoraLog { get; set; }
}