using AceleraPlenoProjetoFinal.Api.Data;
using AceleraPlenoProjetoFinal.Api.Repositories.Interfaces;
using AceleraPlenoProjetoFinal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AceleraPlenoProjetoFinal.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/v1/CargasScheduler")]
public class CargasSchedulerController : ControllerBase
{
    private readonly DataContext _dataContext;
    private AzureBlobContainerService _azureBlobContainerService;
    private ICargaRepository _cargaRepository;
    private string blobPathCompleted;

    public CargasSchedulerController(DataContext dataContext, IConfiguration configuration, ICargaRepository cargaRepository)
    {
        _dataContext = dataContext;
        _azureBlobContainerService = new AzureBlobContainerService(configuration);
        _cargaRepository = cargaRepository;
        blobPathCompleted = configuration.GetValue<string>("AzureBlobContainers:blobPathCompleted");
    }

    [HttpPost]
    [Route("UnidadeInstituicao")]
    public IActionResult InserirUnidadeInstituicao()
    {
        try
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "PENDENTE" && c.TipoArquivo == "UNIDADEINSTITUICAO").ToList();

            if (result.Count == 0)
                return Ok();

            foreach (var blob in result)
            {
                blob.EstadoCarga = "EXECUTANDO";
                blob.DataHoraInicioCarga = DateTime.Now;
                _dataContext.SaveChanges();

                var blobUri = _azureBlobContainerService.GetSasUriBlobContainer($"{blob.CaminhoCargaUpload}/{blob.DescArquivo}");

                var cargaResult = _cargaRepository.InserirUnidadeInstituicao(blobUri);

                if (cargaResult)
                {
                    _azureBlobContainerService.MoveBlobContainer(blob.DescArquivo, blob.CaminhoCargaUpload, blobPathCompleted);

                    blob.EstadoCarga = "OK";
                    blob.DataHoraFimCarga = DateTime.Now;
                    _dataContext.SaveChanges();
                }

                else
                {
                    blob.EstadoCarga = "PENDENTE";
                    blob.DataHoraInicioCarga = null;
                    _dataContext.SaveChanges();
                }
            }

            return Ok();
        }

        catch (Exception ex)
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "EXECUTANDO" && c.TipoArquivo == "UNIDADEINSTITUICAO").FirstOrDefault();

            if (result != null)
            {
                result.EstadoCarga = "PENDENTE";
                result.DataHoraInicioCarga = null;
                result.MensagemErro = ex.Message;
                _dataContext.SaveChanges();
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("OperacaoCaixa")]
    public IActionResult InserirOperacaoCaixa()
    {
        try
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "PENDENTE" && c.TipoArquivo == "OPERACAO").ToList();

            if (result.Count == 0)
                return Ok();

            foreach (var blob in result)
            {
                blob.EstadoCarga = "EXECUTANDO";
                blob.DataHoraInicioCarga = DateTime.Now;
                _dataContext.SaveChanges();

                var blobUri = _azureBlobContainerService.GetSasUriBlobContainer($"{blob.CaminhoCargaUpload}/{blob.DescArquivo}");

                var cargaResult = _cargaRepository.InserirOperacao(blobUri);

                if (cargaResult)
                {
                    _azureBlobContainerService.MoveBlobContainer(blob.DescArquivo, blob.CaminhoCargaUpload, blobPathCompleted);

                    blob.EstadoCarga = "OK";
                    blob.DataHoraFimCarga = DateTime.Now;
                    _dataContext.SaveChanges();
                }

                else
                {
                    blob.EstadoCarga = "PENDENTE";
                    blob.DataHoraInicioCarga = null;
                    _dataContext.SaveChanges();
                }
            }

            return Ok();
        }

        catch (Exception ex)
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "EXECUTANDO" && c.TipoArquivo == "OPERACAO").FirstOrDefault();

            if (result != null)
            {
                result.EstadoCarga = "PENDENTE";
                result.MensagemErro = ex.Message;
                result.DataHoraInicioCarga = null;
                _dataContext.SaveChanges();
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Usuario")]
    public IActionResult Inserirusuario()
    {
        try
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "PENDENTE" && c.TipoArquivo == "USUARIO").ToList();

            if (result.Count == 0)
                return Ok();

            foreach (var blob in result)
            {
                blob.EstadoCarga = "EXECUTANDO";
                blob.DataHoraInicioCarga = DateTime.Now;
                _dataContext.SaveChanges();

                var blobUri = _azureBlobContainerService.GetSasUriBlobContainer($"{blob.CaminhoCargaUpload}/{blob.DescArquivo}");

                var cargaResult = _cargaRepository.InserirUsuario(blobUri);

                if (cargaResult)
                {
                    _azureBlobContainerService.MoveBlobContainer(blob.DescArquivo, blob.CaminhoCargaUpload, blobPathCompleted);

                    blob.EstadoCarga = "OK";
                    blob.DataHoraFimCarga = DateTime.Now;
                    _dataContext.SaveChanges();
                }

                else
                {
                    blob.EstadoCarga = "PENDENTE";
                    blob.DataHoraInicioCarga = null;
                    _dataContext.SaveChanges();
                }
            }

            return Ok();
        }

        catch (Exception ex)
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "EXECUTANDO" && c.TipoArquivo == "USUARIO").FirstOrDefault();

            if (result != null)
            {
                result.EstadoCarga = "PENDENTE";
                result.MensagemErro = ex.Message;
                result.DataHoraInicioCarga = null;
                _dataContext.SaveChanges();
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Terminal")]
    public IActionResult InserirTerminal()
    {
        try
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "PENDENTE" && c.TipoArquivo == "TERMINAL").ToList();

            if (result.Count == 0)
                return Ok();

            foreach (var blob in result)
            {
                blob.EstadoCarga = "EXECUTANDO";
                blob.DataHoraInicioCarga = DateTime.Now;
                _dataContext.SaveChanges();

                var blobUri = _azureBlobContainerService.GetSasUriBlobContainer($"{blob.CaminhoCargaUpload}/{blob.DescArquivo}");

                var cargaResult = _cargaRepository.InserirTerminal(blobUri);

                if (cargaResult)
                {
                    _azureBlobContainerService.MoveBlobContainer(blob.DescArquivo, blob.CaminhoCargaUpload, blobPathCompleted);

                    blob.EstadoCarga = "OK";
                    blob.DataHoraFimCarga = DateTime.Now;
                    _dataContext.SaveChanges();
                }

                else
                {
                    blob.EstadoCarga = "PENDENTE";
                    blob.MensagemErro = "Carga não completada";
                    blob.DataHoraInicioCarga = null;
                    _dataContext.SaveChanges();
                }
            }

            return Ok();
        }

        catch (Exception ex)
        {
            var result = _dataContext.Carga.Where(c => c.EstadoCarga == "EXECUTANDO" && c.TipoArquivo == "TERMINAL").FirstOrDefault();

            if (result != null)
            {
                result.EstadoCarga = "PENDENTE";
                result.MensagemErro = ex.Message;
                result.DataHoraInicioCarga = null;
                _dataContext.SaveChanges();
            }

            return BadRequest(ex.Message);
        }
    }
}