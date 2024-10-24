using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("03t_GRUPOACESSO")]
public class GrupoAcessoModel
{
    [Key]
    [Column("IDGRUPOACESSO")]
    public int IdGrupoAcesso { get; set; }

    [Column("DESCGRUPOACESSO")]
    public string DescGrupoAcesso { get; set; }

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