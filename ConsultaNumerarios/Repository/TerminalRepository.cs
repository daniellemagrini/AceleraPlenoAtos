using ConsultaNumerarios.Data;
using ConsultaNumerarios.Interfaces;
using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Repository
{
    public class TerminalRepository : ITerminalRepository
    {
        private readonly DataContext _context;
        public TerminalRepository(DataContext context)
        {
            _context = context;
        }
        public TerminalModel GetTerminalPorPaENumTerminal(string numPA, int numTerminal)
        {
            return _context.Terminal
                .Where(terminal => terminal.IdUnidadeInst == numPA)
                .Where(terminal => terminal.NumTerminal == numTerminal)
                .FirstOrDefault();
        }

    }
}
