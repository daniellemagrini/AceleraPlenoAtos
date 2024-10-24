using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultaNumerarios.Models;

[Table("06t_TRANSPORTADORAVALORES")]
public class TransportadoraValoresModel
{
    [Key]
    [Column("IDTRANSPORTADORAVALORES")]
    public int IdTransportadoraValores { get; set; }

    [Column("NUMCNPJ")]
    public string Cnpj { get; set; }

    [Column("DESCTRANSPORTADORA")]
    public string DescricaoTransportadora { get; set; }

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