using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Acelera.API.ParamLog.Model;
using Acelera.API.ParamLog.Repository.Interface;

namespace Acelera.API.ParamLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        [HttpGet("GetAllLogs")]
        public async Task<List<Log>> GetAllLogs()
        {
            var resposta = await _logRepository.GetAllLogs();
            return resposta;
        }
    }
}
