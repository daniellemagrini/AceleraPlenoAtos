using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultaNumerarios.Models;

[Table("09t_TIPOOPERACAO")]
public class TipoOperacaoModel
{
    [Key]
    [Column("IDTIPOOPERACAO")]
    public int IdTipoOperacao { get; set; }

    [Column("IDGRUPOCAIXA")]
    public int IdGrupoCaixa { get; set; }

    [Column("IDOPERACAOCAIXA")]
    public int IdOperacaoCaixa { get; set; }

    [Column("OPERACAO")]
    public string Operacao { get; set; }

    [Column("DESCOPERACAO")]
    public string DescricaoOperacao { get; set; }

    [Column("CODHISTORICO")]
    public int CodHistorico { get; set; }

    [Column("DESCHISTORICO")]
    public string DescricaoHistorico { get; set; }

    [Column("SENSIBILIZACAO")]
    public string Sensibilizacao { get; set; }

    [Column("CODCRIADOPOR")]
    public int CodCriadoPor { get; set; }

    [Column("DATAHORACRIACAO")]
    public DateTime? DataHoraCriacao { get; set; }

    [Column("CODALTERADOPOR")]
    public int? CodAlteradoPor { get; set; } = null;

    [Column("DATAHORAALTERACAO")]
    public DateTime? DataHoraAlteracao { get; set; } = null;

    [Column("CODINATIVOPOR")]
    public int? CodInativoPor { get; set; } = null;

    [Column("DATAHORAINATIVO")]
    public DateTime? DataHoraInativo { get; set; } = null;
}