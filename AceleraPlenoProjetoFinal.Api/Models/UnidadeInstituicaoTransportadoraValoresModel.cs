using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("07t_UNIDADEINSTITUICAOTRANSPORTADORAVALORES")]
public class UnidadeInstituicaoTransportadoraValoresModel
{
    [Key]
    [Column("IDUNIDADEINSTTRANSPORTADORAVALORES")]
    public int IdUnidadeInstTransportadoraValores { get; set; }

    [Column("IDTRANSPORTADORAVALORES")]
    public int IdTransportadoraValores { get; set; }

    [Column("IDUNIDADEINST")]
    public string IdUnidadeInst { get; set; }

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