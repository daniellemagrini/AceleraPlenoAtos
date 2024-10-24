using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Interfaces
{
    public interface IOperacaoRepository
    {
        List<OperacaoModel> GetOperacoesPorPa(string idPa);
        List<OperacaoModel> GetOperacoesPorPeriodo(string idPa, DateTime inicio, DateTime fim);
    }
}
