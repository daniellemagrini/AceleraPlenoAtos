using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Acelera.API.AuthOriginal.Model
{
    [Table("05t_UNIDADEINSTITUICAO")]
    public class Unidade
    {
        [Display(Name = "ID")]
        [Key]
        public string IDUNIDADEINST { get; set; }

        [Display(Name = "ID DA INSTITUICAO")]
        public int IDINSTITUICAO { get; set; }

        [Display(Name = "CNPJ")]
        public string? NUMCNPJ { get; set; }

        [Display(Name = "NOME DA UNIDADE")]
        public string NOMEUNIDADE { get; set; }

        [Display(Name = "SIGLA DA UNIDADE")]
        public string SIGLAUNIDADE { get; set; }

        [Display(Name = "DATA DO CADASTRAMENTO")]
        public DateTime? DATACADASTRAMENTO { get; set; }

        [Display(Name = "TIPO INSTITUICAO")]
        public int? CODTIPOINSTITUICAO { get; set; }

        [Display(Name = "TIPO UNIDADE")]
        public int? CODTIPOUNIDADE { get; set; }

        [Display(Name = "INICIO")]
        public DateTime? DATAINICIOSICOOB { get; set; }

        [Display(Name = "FIM")]
        public DateTime? DATAFIMSICOOB { get; set; }

        [Display(Name = "CHECK ALTERACAO")]
        public int? NUMCHECKALTERACAO { get; set; }

        [Display(Name = "UNIDADE INSTITUIÇÃO RESPONSÁVEL")]
        public int? IDUNIDADEINSTRESP { get; set; }

        [Display(Name = "SITUACAO")]
        public int? CODSITUACAOUNID { get; set; }

        [Display(Name = "SIRC")]
        public int? NUMSIRC { get; set; }

        [Display(Name = "INTERNET")]
        public string? DESCENDINTERNET { get; set; }

        [Display(Name = "DATA MARCA")]
        public DateTime? DATAINICIOUTILIZACAOMARCASICOOB { get; set; }

        [Display(Name = "ATENDIMENTO PUBLICO EXTERNO")]
        public int? BOLATENDIMENTOPUBLICOEXTERNO { get; set; }

        [Display(Name = "INSCRICAO MUNICIPAL")]
        public string? NUMINSCRICAOMUNICIPAL { get; set; }

        [Display(Name = "NIRE")]
        public string? NUMNIRE { get; set; }

        [Display(Name = "ID INSTITUICAO INCORPORADORA")]
        public int? IDINSTITUICAOINCORPORADORA { get; set; }

        [Display(Name = "DATA INCORPORACAO")]
        public DateTime? DATAINCORPORACAO { get; set; }

        [Display(Name = "UTILIZA COMPARTILHAMENTO")]
        public int? BOLUTILIZACOMPARTILHAMENTO { get; set; }

        [Display(Name = "DATA INICIO FUNCIONAMENTO")]
        public DateTime? DATAINICIOFUNCIONAMENTO { get; set; }

        [Display(Name = "DATA FIM FUNCIONAMENTO")]
        public DateTime? DATAFIMFUNCIONAMENTO { get; set; }

        [Display(Name = "UTILIZA SISBR")]
        public int? BOLUTILIZASISBR { get; set; }

        [Display(Name = "DATA INICIO UTILIZA SISBR")]
        public DateTime? DATAINICIOUTILIZASISBR { get; set; }

        [Display(Name = "DATA FIM UTILIZA SISBR")]
        public DateTime? DATAFIMUTILIZASISBR { get; set; }

        [Display(Name = "ISENTO INSCRICAO MUNICIPAL")]
        public int? BOLISENTOINSCRICAOMUNICIPAL { get; set; }

        [Display(Name = "ISENTO NIRE")]
        public int? BOLISENTONIRE { get; set; }

        [Display(Name = "SINALIZADO")]
        public int? BOLSINALIZADOSICOOB { get; set; }

        [Display(Name = "PA INCORPORADO")]
        public int? BOLPAINCORPORADO { get; set; }

        [Display(Name = "DATA HORA CARGA")]
        public DateTime? DATAHORACARGA { get; set; }

        [Display(Name = "CRIADO POR")]
        public int CODCRIADOPOR { get; set; }

        [Display(Name = "DATA HORA CRIACAO")]
        public DateTime DATAHORACRIACAO { get; set; }

        [Display(Name = "ALTERADO POR")]
        public int? CODALTERADOPOR { get; set; }

        [Display(Name = "DATA HORA ALTERACAO")]
        public DateTime? DATAHORAALTERACAO { get; set; }

        [Display(Name = "INATIVO POR")]
        public int? CODINATIVOPOR { get; set; }

        [Display(Name = "DATA HORA INATIVO")]
        public DateTime? DATAHORAINATIVO { get; set; }

    }
}
