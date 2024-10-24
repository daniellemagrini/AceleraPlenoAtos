using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("01t_CARGA")]
public class CargaModel
{
    [Key]
    [Column("IDCARGA")]
    public int IdCarga { get; set; }

    [Column("IDUSUARIOSISTEMA")]
    public int IdUsuarioSistema { get; set; }

    [Column("DESCARQUIVO")]
    public string DescArquivo { get; set; }

    [Column("TIPOARQUIVO")]
    public string? TipoArquivo { get; set; }

    [Column("CAMINHOCARGAUPLOAD")]
    public string? CaminhoCargaUpload { get; set; }

    [Column("ESTADOUPLOAD")]
    public string EstadoUpload { get; set; }

    [Column("ESTADOCARGA")]
    public string? EstadoCarga { get; set; }

    [Column("MENSAGEMERRO")]
    public string? MensagemErro { get; set; } = null;

    [Column("DATAHORAUPLOAD")]
    public DateTime DataHoraUpload { get; set; }

    [Column("DATAHORAINICIOCARGA")]
    public DateTime? DataHoraInicioCarga { get; set; } = null;

    [Column("DATAHORAFIMCARGA")]
    public DateTime? DataHoraFimCarga { get; set; } = null;
}