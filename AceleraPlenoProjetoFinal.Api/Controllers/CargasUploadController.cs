using AceleraPlenoProjetoFinal.Api.Data;
using AceleraPlenoProjetoFinal.Api.Models;
using AceleraPlenoProjetoFinal.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AceleraPlenoProjetoFinal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/CargasUpload")]
public class CargasUploadController : ControllerBase
{
    private readonly DataContext _dataContext;
    private AzureBlobContainerService _azureBlobContainerService;

    public CargasUploadController(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _azureBlobContainerService = new AzureBlobContainerService(configuration);
    }

    [HttpPost]
    [Route("UnidadeInstituicao")]
    public IActionResult InserirUnidadeInstituicao([FromForm] CargasUploadModel arquivoUpload)
    {
        var cargaObj = new CargaModel();

        var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        string fileName = "";

        try
        {
            if (arquivoUpload?.Arquivo == null || arquivoUpload.Arquivo.Length == 0)
                return NotFound("Arquivo não encontrado.");

            // Verifique a extensão do arquivo
            if (!Path.GetExtension(arquivoUpload.Arquivo.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Apenas arquivos .xlsx são permitidos.");

            var result = _dataContext.Parametrizacao.Where(p => p.TipoCarga == "UNIDADEINSTITUICAO" && p.DataHoraInativo == null).FirstOrDefault();

            if (result != null)
            {
                fileName = _azureBlobContainerService.UploadBlobContainer(arquivoUpload, result.CaminhoCarga);

                cargaObj.IdUsuarioSistema = idUsuarioLogado;
                cargaObj.DescArquivo = fileName;
                cargaObj.TipoArquivo = "UNIDADEINSTITUICAO";
                cargaObj.CaminhoCargaUpload = result.CaminhoCarga;
                cargaObj.EstadoUpload = "OK";
                cargaObj.EstadoCarga = "PENDENTE";
                cargaObj.DataHoraUpload = DateTime.Now;

                _dataContext.Add(cargaObj);
                _dataContext.SaveChanges();

                return Ok("Arquivo enviado com sucesso.");
            }

            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "UNIDADEINSTITUICAO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = "Parametrização não encontrada.";
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return NotFound("Parametrização não encontrada.");
        }

        catch (Exception ex)
        {
            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "UNIDADEINSTITUICAO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = ex.Message;
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Operacao")]
    public IActionResult InserirOperacao([FromForm] CargasUploadModel arquivoUpload)
    {
        var cargaObj = new CargaModel();
        var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        string fileName = "";

        try
        {
            if (arquivoUpload?.Arquivo == null || arquivoUpload.Arquivo.Length == 0)
                return NotFound("Arquivo não encontrado.");

            // Verifique a extensão do arquivo
            if (!Path.GetExtension(arquivoUpload.Arquivo.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Apenas arquivos .csv são permitidos.");

            var result = _dataContext.Parametrizacao.Where(p => p.TipoCarga == "OPERACAO" && p.DataHoraInativo == null).FirstOrDefault();

            if (result != null)
            {
                fileName = _azureBlobContainerService.UploadBlobContainer(arquivoUpload, result.CaminhoCarga);

                cargaObj.IdUsuarioSistema = idUsuarioLogado;
                cargaObj.DescArquivo = fileName;
                cargaObj.TipoArquivo = "OPERACAO";
                cargaObj.CaminhoCargaUpload = result.CaminhoCarga;
                cargaObj.EstadoUpload = "OK";
                cargaObj.EstadoCarga = "PENDENTE";
                cargaObj.DataHoraUpload = DateTime.Now;

                _dataContext.Add(cargaObj);
                _dataContext.SaveChanges();

                return Ok("Arquivo enviado com sucesso.");
            }

            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "OPERACAO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = "Parametrização não encontrada.";
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return NotFound("Parametrização não encontrada.");
        }

        catch (Exception ex)
        {
            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "OPERACAO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = ex.Message;
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Usuario")]
    public IActionResult InserirUsuario([FromForm] CargasUploadModel arquivoUpload)
    {
        var cargaObj = new CargaModel();
        var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        string fileName = "";

        try
        {
            if (arquivoUpload?.Arquivo == null || arquivoUpload.Arquivo.Length == 0)
                return NotFound("Arquivo não encontrado.");

            // Verifique a extensão do arquivo
            if (!Path.GetExtension(arquivoUpload.Arquivo.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Apenas arquivos .xlsx são permitidos.");

            var result = _dataContext.Parametrizacao.Where(p => p.TipoCarga == "USUARIO" && p.DataHoraInativo == null).FirstOrDefault();

            if (result != null)
            {
                fileName = _azureBlobContainerService.UploadBlobContainer(arquivoUpload, result.CaminhoCarga);

                cargaObj.IdUsuarioSistema = idUsuarioLogado;
                cargaObj.DescArquivo = fileName;
                cargaObj.TipoArquivo = "USUARIO";
                cargaObj.CaminhoCargaUpload = result.CaminhoCarga;
                cargaObj.EstadoUpload = "OK";
                cargaObj.EstadoCarga = "PENDENTE";
                cargaObj.DataHoraUpload = DateTime.Now;

                _dataContext.Add(cargaObj);
                _dataContext.SaveChanges();

                return Ok("Arquivo enviado com sucesso.");
            }

            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "USUARIO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = "Parametrização não encontrada.";
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return NotFound("Parametrização não encontrada.");
        }

        catch (Exception ex)
        {
            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "USUARIO";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = ex.Message;
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Terminal")]
    public IActionResult InserirTerminal([FromForm] CargasUploadModel arquivoUpload)
    {
        var cargaObj = new CargaModel();
        var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        string fileName = "";

        try
        {
            if (arquivoUpload?.Arquivo == null || arquivoUpload.Arquivo.Length == 0)
                return NotFound("Arquivo não encontrado.");

            // Verifique a extensão do arquivo
            if (!Path.GetExtension(arquivoUpload.Arquivo.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Apenas arquivos .xlsx são permitidos.");

            var result = _dataContext.Parametrizacao.Where(p => p.TipoCarga == "TERMINAL" && p.DataHoraInativo == null).FirstOrDefault();

            if (result != null)
            {
                fileName = _azureBlobContainerService.UploadBlobContainer(arquivoUpload, result.CaminhoCarga);

                cargaObj.IdUsuarioSistema = idUsuarioLogado;
                cargaObj.DescArquivo = fileName;
                cargaObj.TipoArquivo = "TERMINAL";
                cargaObj.CaminhoCargaUpload = result.CaminhoCarga;
                cargaObj.EstadoUpload = "OK";
                cargaObj.EstadoCarga = "PENDENTE";
                cargaObj.DataHoraUpload = DateTime.Now;

                _dataContext.Add(cargaObj);
                _dataContext.SaveChanges();

                return Ok("Arquivo enviado com sucesso.");
            }

            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "TERMINAL";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = "Parametrização não encontrada.";
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return NotFound("Parametrização não encontrada.");
        }

        catch (Exception ex)
        {
            cargaObj.IdUsuarioSistema = idUsuarioLogado;
            cargaObj.DescArquivo = arquivoUpload.Arquivo.FileName;
            cargaObj.TipoArquivo = "TERMINAL";
            cargaObj.CaminhoCargaUpload = null;
            cargaObj.EstadoUpload = "ERRO";
            cargaObj.EstadoCarga = null;
            cargaObj.MensagemErro = ex.Message;
            cargaObj.DataHoraUpload = DateTime.Now;

            _dataContext.Add(cargaObj);
            _dataContext.SaveChanges();

            return BadRequest(ex.Message);
        }
    }
}