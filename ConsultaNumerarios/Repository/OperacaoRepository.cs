using ConsultaNumerarios.Data;
using ConsultaNumerarios.Interfaces;
using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Repository
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly DataContext _context;
        public OperacaoRepository(DataContext context)
        {
            _context = context;
        }

        public List<OperacaoModel> GetOperacoesPorPa(string idPa)
        {
            return [.. _context.Operacao.Where(op => op.IdUnidadeInst == idPa)];
        }
        public List<OperacaoModel> GetOperacoesPorPeriodo(string idPa, DateTime inicio, DateTime fim)
        {
            return [.. _context.Operacao.Where(op => op.IdUnidadeInst == idPa && op.DataOperacao >= inicio && op.DataOperacao <= fim)];
        }
    }
}
