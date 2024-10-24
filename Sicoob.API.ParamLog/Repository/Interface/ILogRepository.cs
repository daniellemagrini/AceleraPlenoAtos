using Acelera.API.ParamLog.Model;

namespace Acelera.API.ParamLog.Repository.Interface
{
    public interface ILogRepository
    {
        Task<List<Log>> GetAllLogs();
    }
}
