using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models;

[Table("12t_TERMINAL")]
public class TerminalModel
{
    [Key]
    [Column("IDTERMINAL")]
    public int IdTerminal { get; set; }

    [Column("IDUNIDADEINST")]
    public string IdUnidadeInst { get; set; }

    [Column("IDTIPOTERMINAL")]
    public int IdTipoTerminal { get; set; }

    [Column("IDUSUARIO")]
    public string IdUsuario { get; set; }

    [Column("IDUSUARIOLIBERACAO")]
    public string IdUsuarioLiberacao { get; set; }

    [Column("IDINSTITUICAO")]
    public int IdInstituicao { get; set; }

    [Column("IDPRODUTO")]
    public int IdProduto { get; set; }

    [Column("DATAPROCESSAMENTO")]
    public DateTime DataProcessamento { get; set; }

    [Column("NUMTERMINAL")]
    public int NumTerminal { get; set; }

    [Column("IDSITUACAOTERMINAL")]
    public int IdSituacaoTerminal { get; set; }

    [Column("DESCESTTRABALHO")]
    public string? DescEstTrabalho { get; set; }

    [Column("NUMULTAUTENTICACAO")]
    public int NumUltAutenticacao { get; set; }

    [Column("NUMLOTECCO")]
    public int NumLoteCco { get; set; }

    [Column("MENORVALORNOTA")]
    public decimal MenorValorNota { get; set; }

    [Column("DATAHORALIBERACAO")]
    public DateTime DataHoraLiberacao { get; set; }

    [Column("NUMLOTECHEQUE")]
    public int NumLoteCheque { get; set; }

    [Column("NUMULTSEQLANCCCO")]
    public int NumUltSeqLancCco { get; set; }

    [Column("NUMULTREMESSA")]
    public int NumUltRemessa { get; set; }

    [Column("IDCLIENTECOR")]
    public int? IdClienteCor { get; set; }

    [Column("NUMLOTEDOC")]
    public int NumLoteDoc { get; set; }

    [Column("NUMULTSEQDOC")]
    public int NumUltSeqDoc { get; set; }

    [Column("DESCVERSAOSO")]
    public string? DescVersaoSo { get; set; }

    [Column("DESCMEMORIARAM")]
    public string? DescMemoriaRam { get; set; }

    [Column("DESCESPACODISCO")]
    public string? DescEspacoDisco { get; set; }

    [Column("DESCPACOTESERVICO")]
    public string? DescPacoteServico { get; set; }

    [Column("NUMLOTEDEC")]
    public int NumLoteDec { get; set; }

    [Column("NUMULTSEQDEC")]
    public int NumUltSeqDec { get; set; }

    [Column("VALORLIMITESAQUE")]
    public decimal ValorLimiteSaque { get; set; }

    [Column("VALORLIMITETERMINAL")]
    public decimal ValorLimiteTerminal { get; set; }

    [Column("NUMTESOUREIRO")]
    public int? NumTesoureiro { get; set; }

    [Column("NUMIPTESOUREIRO")]
    public string? NumIpTesoureiro { get; set; }

    [Column("CODTIPOBALANCEAMENTO")]
    public int? CodTipoBalanceamento { get; set; }

    [Column("NUMTIMEOUTDISPENSADOR")]
    public int? NumTimeOutDispensador { get; set; }

    [Column("CODLADODEPOSITARIO")]
    public int? CodLadoDepositario { get; set; }

    [Column("NUMPORTATESOUREIRO")]
    public int? NumPortaTesoureiro { get; set; }

    [Column("NUMCHEKALTERACAO")]
    public int? NumCheckAlteracao { get; set; }

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