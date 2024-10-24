using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("00t_PARAMETRIZACAO")]
public class ParametrizacaoModel
{
    [Key]
    [Column("IDPARAMETRIZACAO")]
    public int IdParametrizacao { get; set; }

    [Column("TIPOCARGA")]
    public string TipoCarga { get; set; }

    [Column("CAMINHOCARGA")]
    public string CaminhoCarga { get; set; }

    [Column("INTERVALOEXECUCAO")]
    public int IntervaloExecucao { get; set; }

    [Column("CODCRIADOPOR")]
    public int CodCriadoPor { get; set; }

    [Column("DATAHORACRIACAO")]
    public DateTime DataHoraCriacao { get; set; }

    [Column("CODALTERADOPOR")]
    public int? CodAlteradoPor { get; set; } = null;

    [Column("DATAHORAALTERACAO")]
    public DateTime? DataHoraAlteracao { get; set; } = null;

    [Column("CODINATIVOPOR")]
    public int? CodInativoPor { get; set; } = null;

    [Column("DATAHORAINATIVO")]
    public DateTime? DataHoraInativo { get; set; } = null;
}