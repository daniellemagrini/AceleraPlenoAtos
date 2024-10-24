using ConsultaNumerarios.Dto;

namespace ConsultaNumerarios.Interfaces
{
    public interface ITerminalService
    {
        SaldoDeTerminais CalculaSaldosDeTerminais(string operacaoModels);
        SaldoPorPeriodo.Response CalculaSaldosPorPeriodo(SaldoPorPeriodo.Request request);
        string ValidaPeriodo(DateTime inicio, DateTime fim);
    }
}
