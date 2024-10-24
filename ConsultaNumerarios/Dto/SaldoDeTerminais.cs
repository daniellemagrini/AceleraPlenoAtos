namespace ConsultaNumerarios.Dto
{
    public class SaldoDeTerminais
    {
        public SaldoDeTerminais()
        {
            Terminal = new List<Terminal>();
        }
        public DateTime? UltimaAtualizacao { get; set; }
        public decimal SaldoTotal { get; set; }
        public List<Terminal> Terminal { get; set; }
    }

    public class Terminal
    {
        public int NumTerminal { get; set; }
        public string TipoTerminal { get; set; }
        public decimal Saldo { get; set; }
        public int LimiteMax { get; set; }
        public int LimiteMin { get; set; }
        public string Usuario { get; set; }
        public bool DentroDoLimite { get; set; }
    }
}
