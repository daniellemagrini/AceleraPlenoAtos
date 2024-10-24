using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Interfaces
{
    public interface ITerminalRepository
    {
        TerminalModel GetTerminalPorPaENumTerminal(string numPa, int numTerminal);
    }
}
