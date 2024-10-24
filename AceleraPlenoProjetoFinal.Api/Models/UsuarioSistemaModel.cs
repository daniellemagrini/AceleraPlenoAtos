using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("02t_USUARIOSISTEMA")]
public class UsuarioSistemaModel
{
    [Key]
    [Column("IDUSUARIOSISTEMA")]
    public int IdUsuarioSistema { get; set; }

    [Column("IDUSUARIO")]
    public string IdUsuario { get; set; }

    [Column("LOGIN")]
    public string Login { get; set; }

    [Column("SENHAHASH")]
    public byte? SenhaHash { get; set; }

    [Column("SENHASALT")]
    public byte? SenhaSalt { get; set; }

    [Column("SECRETKEY")]
    public string SecretKey { get; set; }

    [Column("BOLPRIMEIROLOGIN")]
    public int BolPrimeiroLogin { get; set; }

    [Column("ULTIMOLOGIN")]
    public DateTime? UltimoLogin { get; set; } = null;

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