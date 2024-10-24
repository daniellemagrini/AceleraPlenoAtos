using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AceleraPlenoProjetoFinal.Api.Models
{
    [Table("08t_TIPOTERMINAL")]
    public class TipoTerminalModel
    {
        [Key]
        [Column("IDTIPOTERMINAL")]
        public int IdTipoTerminal { get; set; }

        [Column("IDUNIDADEINST")]
        public string IdUnidadeInst { get; set; }
        
        [Column("DESCTIPOTERMINAL")]
        public string DescricaoTipoTerminal { get; set; }
        
        [Column("BOLACESSOLIBERADO")]
        public int BolAcessoLiberado { get; set; }
        
        [Column("NUMCHEKALTERACAO")]
        public int NumCheckAlteracao { get; set; }
        
        [Column("LIMSUPERIOR")]
        public int LimiteSuperior { get; set; }
        
        [Column("LIMINFERIOR")]
        public int LimiteInferior { get; set; }

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
}