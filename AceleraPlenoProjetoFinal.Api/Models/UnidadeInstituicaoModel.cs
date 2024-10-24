using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("05t_UNIDADEINSTITUICAO")]
public class UnidadeInstituicaoModel
{
    [Key]
    [Column("IDUNIDADEINST")]
    public string IdUnidadeInstituicao { get; set; }

    [Column("IDINSTITUICAO")]
    public int IdInstituicao { get; set; }

    [Column("NUMCNPJ")]
    public string? Cnpj { get; set; }

    [Column("NOMEUNIDADE")]
    public string NomeUnidade { get; set; }

    [Column("SIGLAUNIDADE")]
    public string SiglaUnidade { get; set; }

    [Column("DATACADASTRAMENTO")]
    public DateTime? DataCadastramento { get; set; }

    [Column("CODTIPOINSTITUICAO")]
    public int CodTipoInstituicao { get; set; }

    [Column("CODTIPOUNIDADE")]
    public int CodTipoUnidade { get; set; }

    [Column("DATAINICIOSICOOB")]
    public DateTime? DataInicioSicoob { get; set; }

    [Column("DATAFIMSICOOB")]
    public DateTime? DataFimSicoob { get; set; }

    [Column("NUMCHECKALTERACAO")]
    public int NumCheckAlteracao { get; set; }

    [Column("IDUNIDADEINSTRESP")]
    public int? IdUnidadeInstResp { get; set; }

    [Column("CODSITUACAOUNID")]
    public int CodSituacaoUnid { get; set; }

    [Column("NUMSIRC")]
    public int? NumSirc { get; set; }

    [Column("DESCENDINTERNET")]
    public string? DescricaoEndInternet { get; set; }

    [Column("DATAINICIOUTILIZACAOMARCASICOOB")]
    public DateTime? DataInicioUtilizacaoMarcaSicoob { get; set; }

    [Column("BOLATENDIMENTOPUBLICOEXTERNO")]
    public int? BolAtentimentoPublicoExterno { get; set; }

    [Column("NUMINSCRICAOMUNICIPAL")]
    public string? NumInscricaoMunicipal { get; set; }

    [Column("NUMNIRE")]
    public string? NumNire { get; set; }

    [Column("IDINSTITUICAOINCORPORADORA")]
    public int? IdInstituicaoIncorporadora { get; set; }

    [Column("DATAINCORPORACAO")]
    public DateTime? DataIncorporacao { get; set; }

    [Column("BOLUTILIZACOMPARTILHAMENTO")]
    public int? BolUtilizaCompartilhamento { get; set; }

    [Column("DATAINICIOFUNCIONAMENTO")]
    public DateTime? DataInicioFuncionamento { get; set; }

    [Column("DATAFIMFUNCIONAMENTO")]
    public DateTime? DataFimFuncionamento { get; set; }

    [Column("BOLUTILIZASISBR")]
    public int BolUtilizaSisbr { get; set; }

    [Column("DATAINICIOUTILIZASISBR")]
    public DateTime? DataInicioUtilizaSisbr { get; set; }

    [Column("DATAFIMUTILIZASISBR")]
    public DateTime? DataFimUtilizaSisbr { get; set; }

    [Column("BOLISENTOINSCRICAOMUNICIPAL")]
    public int? BolIsentoInscricaoMunicipal { get; set; }

    [Column("BOLISENTONIRE")]
    public int? BolIsentoNire { get; set; }

    [Column("BOLSINALIZADOSICOOB")]
    public int? BolSinalizadoSicoob { get; set; }

    [Column("BOLPAINCORPORADO")]
    public int? BolPaIncorporado { get; set; }

    [Column("DATAHORACARGA")]
    public DateTime DataHoraCarga { get; set; }

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