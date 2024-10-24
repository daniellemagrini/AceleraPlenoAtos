using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("04t_USUARIOSISTEMAGRUPOACESSO")]
public class UsuarioSistemaGrupoAcesso
{
    [Key]
    [Column("IDUSUARIOSISTEMAGRUPOACESSO")]
    public int IDUSUARIOSISTEMAGRUPOACESSO { get; set; }

    [Column("IDUSUARIOSISTEMA")]
    public int IDUSUARIOSISTEMA { get; set; }

    [Column("IDGRUPOACESSO")]
    public int IDGRUPOACESSO { get; set; }

    [Column("CODCRIADOPOR")]
    public int CODCRIADOPOR { get; set; }

    [Column("DATAHORACRIACAO")]
    public DateTime? DATAHORACRIACAO { get; set; }

    [Column("CODALTERADOPOR")]
    public int? CODALTERADOPOR { get; set; } = null;

    [Column("DATAHORAALTERACAO")]
    public DateTime? DATAHORAALTERACAO { get; set; } = null;

    [Column("CODINATIVOPOR")]
    public int? CODINATIVOPOR { get; set; } = null;

    [Column("DATAHORAINATIVO")]
    public DateTime? DATAHORAINATIVO { get; set; } = null;
}