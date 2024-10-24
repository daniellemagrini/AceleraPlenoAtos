using ConsultaNumerarios.Data;
using ConsultaNumerarios.Interfaces;
using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Repository
{
    public class TipoTerminalRepository : ITipoTerminalRepository
    {
        private readonly DataContext _context;
        public TipoTerminalRepository(DataContext context)
        {
            _context = context;
        }
        public List<TipoTerminalModel> GetTiposDeTerminais()
        {
            return [.. _context.TipoTerminal];
        }
    }
}
