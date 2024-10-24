using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultaNumerarios.Api.Models;

[Table("11t_USUARIO")]
public class UsuarioModel
{
    [Key]
    [Column("IDUSUARIO")]
    public string IdUsuario { get; set; }

    [Column("IDUNIDADEINST")]
    public string IdUnidadeInst { get; set; }

    [Column("IDINSTITUICAO")]
    public int IdInstituicao { get; set; }

    [Column("NUMCHECKALTERACAO")]
    public int NumCheckAlteracao { get; set; }

    [Column("IDINSTITUICAOUSUARIO")]
    public int IdInstituicaoUsuario { get; set; }

    [Column("DESCNOMEUSUARIO")]
    public string? DescNomeUsuario { get; set; }

    [Column("DESCEMAIL")]
    public string? DescEmail { get; set; }

    [Column("BOLHABILITADOUSUARIO")]
    public int BolHabilitadoUsuario { get; set; }

    [Column("DESCSTATUSUSUARIO")]
    public string? DescStatusUsuario { get; set; }

    [Column("BOLVERIFICANOMEMAQUINA")]
    public int BolVerificaNomeMaquina { get; set; }

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