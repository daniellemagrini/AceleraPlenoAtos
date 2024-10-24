using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultaNumerarios.Models;

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
}