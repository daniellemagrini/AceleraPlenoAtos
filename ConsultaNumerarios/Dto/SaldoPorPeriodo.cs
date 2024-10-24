namespace ConsultaNumerarios.Dto
{
    public class SaldoPorPeriodo
    {
        public class Request
        {
            public string idPa { get; set; }
            public int NumTerminal { get; set; }
            public DateTime Inicio { get; set; }
            public DateTime Fim { get; set; }
        }

        public class Response
        {
            public Response()
            {
                DatasOperacao = new List<DataOperacao>();
            }

            public List<DataOperacao> DatasOperacao { get; set; }

            public class DataOperacao
            {
                public DateTime Data { get; set; }
                public decimal Saldo { get; set; }
            }
        }
    }
}
