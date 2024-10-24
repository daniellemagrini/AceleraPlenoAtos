using ConsultaNumerarios.Models;

namespace ConsultaNumerarios.Interfaces
{
    public interface ITipoTerminalRepository
    {
        List<TipoTerminalModel> GetTiposDeTerminais();
    }
}
