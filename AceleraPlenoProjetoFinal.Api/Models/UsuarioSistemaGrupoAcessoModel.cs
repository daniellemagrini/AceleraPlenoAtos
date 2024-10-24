using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("04t_USUARIOSISTEMAGRUPOACESSO")]
public class UsuarioSistemaGrupoAcessoModel
{
    [Key]
    [Column("IDUSUARIOSISTEMAGRUPOACESSO")]
    public int IdUsuarioSistemaGrupoAcesso { get; set; }

    [Column("IDUSUARIOSISTEMA")]
    public int IdUsuarioSistema { get; set; }

    [Column("IDGRUPOACESSO")]
    public int IdGrupoAcesso { get; set; }

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