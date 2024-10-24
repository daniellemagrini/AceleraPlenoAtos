using Microsoft.EntityFrameworkCore;
using Acelera.API.ParamLog.Data;
using Acelera.API.ParamLog.Model;
using Acelera.API.ParamLog.Repository.Interface;

namespace Acelera.API.ParamLog.Repository
{
    public class LogRepository: ILogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método que busca todos os Logs do sistema.
        /// </summary> 
        public async Task<List<Log>> GetAllLogs()
        {
            try
            {
                return await _context.Logs.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de Logs. Tente novamente mais tarde!");
            }
        }
    }
}
