using ConsultaNumerarios.Dto;
using ConsultaNumerarios.Interfaces;

namespace ConsultaNumerarios.Services
{
    public class TerminalService : ITerminalService
    {
        private readonly IOperacaoRepository _operacao;
        private readonly ITerminalRepository _terminal;
        private readonly ITipoTerminalRepository _tipoTerminal;
        private readonly IUsuarioRepository _usuario;
        public TerminalService(IOperacaoRepository operacao, ITerminalRepository terminal, ITipoTerminalRepository tipoTerminal, IUsuarioRepository usuario)
        {
            _operacao = operacao;
            _terminal = terminal;
            _tipoTerminal = tipoTerminal;
            _usuario = usuario;
        }
        /// <summary>
        /// Calcula o saldo dos terminais de um PA
        /// </summary>
        /// <param name="numPA">Número identificador do PA</param>
        /// <returns>Retorna um objeto contendo saldo total e uma lista de cada terminal com seu saldo individual</returns>
        public SaldoDeTerminais CalculaSaldosDeTerminais(string numPA)
        {
            var retorno = new SaldoDeTerminais();

            //Recupera lista de operações relacionadas ao PA selecionado
            var operacoes = _operacao.GetOperacoesPorPa(numPA);

            if (operacoes == null)
                return retorno;

            //Recupera os tipos de terminais
            var listaTiposTerminais = _tipoTerminal.GetTiposDeTerminais();

            //Extrai uma lista distinta de todos os terminais
            var listaTerminais = operacoes.Select(op => op.Terminal).Distinct().ToList();

            //Itera pela lista preenchendo os terminais e saldos no objeto de retorno
            foreach (var terminalIteracao in listaTerminais)
            {
                var TerminalAdd = new Terminal();

                //Separa o número do Terminal
                var numTerminal = int.Parse(terminalIteracao?.Split("/")[2]);
                var terminal = _terminal.GetTerminalPorPaENumTerminal(numPA, numTerminal);
                
                if (terminal == null) 
                    continue;

                //Recupera o usuário registrado no terminal
                var usuario = _usuario.GetUsuarioPorId(terminal.IdUsuario);
                TerminalAdd.Usuario = (usuario != null && usuario.DescNomeUsuario != null) ? usuario.DescNomeUsuario : "Usuário não encontrado";

                TerminalAdd.NumTerminal = numTerminal;
                TerminalAdd.TipoTerminal = listaTiposTerminais
                    .Where(tipoT => tipoT.IdTipoTerminal == terminal.IdTipoTerminal)
                    .Select(tipoT => tipoT.DescricaoTipoTerminal)
                    .FirstOrDefault("Tipo do Terminal não encontrado");

                //Calcula o saldo do terminal
                var operacoesSelecionadas = operacoes.Where(op => op.Terminal == terminalIteracao).ToList();
                operacoesSelecionadas.ForEach(op =>
                {
                    if (op.Sensibilizacao == "+")
                        TerminalAdd.Saldo += op.Valor;
                    if (op.Sensibilizacao == "-")
                        TerminalAdd.Saldo -= op.Valor;
                });

                //Recupera limites min e max, 
                (TerminalAdd.LimiteMin, TerminalAdd.LimiteMax) = listaTiposTerminais
                    .Where(tt => tt.IdTipoTerminal == terminal.IdTipoTerminal)
                    .Select(tt => (tt.LimiteInferior, tt.LimiteSuperior))
                    .FirstOrDefault();

                //Verifica se o saldo está dentro do limite
                TerminalAdd.DentroDoLimite = !(TerminalAdd.Saldo < TerminalAdd.LimiteMin || TerminalAdd.Saldo > TerminalAdd.LimiteMax);

                retorno.Terminal.Add(TerminalAdd);
                retorno.SaldoTotal += TerminalAdd.Saldo;
            }
            //Armazena a data da última operação registrada naquele PA
            retorno.UltimaAtualizacao = operacoes.Max(op => op.DataHoraCriacao);

            return retorno;
        }


        public SaldoPorPeriodo.Response CalculaSaldosPorPeriodo(SaldoPorPeriodo.Request request)
        {
            var retorno = new SaldoPorPeriodo.Response();

            var operacoes = _operacao.GetOperacoesPorPeriodo(request.idPa, request.Inicio.Date, request.Fim.Date);
            if (operacoes == null)
                return retorno;

            var operacoesSelecionadas = operacoes
                .Where(op => int.Parse(op.Terminal?.Split("/")[2]) == request.NumTerminal)
                .GroupBy(op => op.DataOperacao);

            foreach (var data in operacoesSelecionadas)
            {
                var operacao = new SaldoPorPeriodo.Response.DataOperacao();
                operacao.Data = data.Key;
                var valores = data.Select(dt => (dt.Valor, dt.Sensibilizacao));

                foreach (var (valor, sensibilizacao) in valores)
                {
                    if (sensibilizacao == "+")
                        operacao.Saldo += valor;
                    if (sensibilizacao == "-")
                        operacao.Saldo -= valor;
                }

                retorno.DatasOperacao.Add(operacao);
            }

            return retorno;
        }
        /// <summary>
        /// Valida o período de busca solicitado
        /// </summary>
        /// <param name="inicio">Data de início para busca</param>
        /// <param name="fim">Data de fim da busca</param>
        /// <returns>Em caso de períodos inválidos retorna uma mensagem de erro</returns>
        public string ValidaPeriodo(DateTime inicio, DateTime fim)
        {
            if (inicio > DateTime.Today)
                return "A data de início não pode ser uma data futura";

            if (inicio > fim)
                return "A data de início precisa ser menor que a data final";
            
            int dateDif = (fim.Subtract(inicio)).Days;
            if (dateDif <= 3 || dateDif >= 30)
                return "Insira um período entre 3 e 30 dias";

            return "";
        }
    }
}
