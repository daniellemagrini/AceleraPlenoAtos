using ConsultaNumerarios.Dto;
using ConsultaNumerarios.Interfaces;
using ConsultaNumerarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNumerarios.Controllers
{
    [ApiController]
    [Route("api/v0.1/[controller]")]
    [Authorize]
    public class NumerariosController : ControllerBase
    {

        private readonly IUnidadeInstituicao _pa;
        private readonly ITerminalService _terminalService;
        private readonly ILogger<NumerariosController> _logger;

        public NumerariosController(ILogger<NumerariosController> logger, IUnidadeInstituicao pa, ITerminalService terminalService)
        {
            _logger = logger;
            _pa = pa;
            _terminalService = terminalService;
        }

        //TODO: Testar, add um dto
        [HttpGet("ListaDePas")]
        public IActionResult GetPasAtivos()
        {
            //List<UnidadeInstituicaoModel> unidades = _pa.GetUnidadeInstituicao();
            List<Pa> unidades = _pa.GetUnidadeInstituicao();
            if (unidades.Count == 0)
                return NotFound("Erro na busca de PAs");

            return Ok(unidades);
        }

        [HttpGet("SaldoDeTerminais{idPa}")]
        public IActionResult GetSaldosDosTerminais([FromRoute] string idPa)
        {
            if (String.IsNullOrEmpty(idPa))
                return BadRequest("ID de busca inválido");

            var saldos = _terminalService.CalculaSaldosDeTerminais(idPa);
            if (saldos.Terminal.Count() > 0)
                return Ok(saldos);

            else
                return NotFound("Não foram encontrados saldos em Terminais para o PA solicitado");
        }

        [HttpPost("SaldoPorPeriodo")]
        public IActionResult GetSaldosPorPeriodo([FromBody] SaldoPorPeriodo.Request request)
        {
            if (String.IsNullOrEmpty(request.idPa) || (request.NumTerminal == -1))
                return BadRequest("ID de busca inválido");

            string msgErroData = _terminalService.ValidaPeriodo(request.Inicio.Date, request.Fim.Date);
            if (!String.IsNullOrEmpty(msgErroData))
                return BadRequest(msgErroData);

            SaldoPorPeriodo.Response response = _terminalService.CalculaSaldosPorPeriodo(request);
            if (response.DatasOperacao.Count() > 0)
                return Ok(response);

            else
                return NotFound("Não foram encontrados saldos para os períodos selecionados");
        }

    }
}
