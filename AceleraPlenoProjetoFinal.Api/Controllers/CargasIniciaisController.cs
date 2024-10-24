using AceleraPlenoProjetoFinal.Api.Data;
using AceleraPlenoProjetoFinal.Api.Models;
using AceleraPlenoProjetoFinal.Api.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Claims;

namespace AceleraPlenoProjetoFinal.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/CargasIniciais")]
public class CargasIniciaisController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly ValidarDadosCarga _validar;

    public CargasIniciaisController(DataContext dataContext)
    {
        _dataContext = dataContext;
        _validar = new ValidarDadosCarga();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("UsuarioSistema")]
    public IActionResult InserirUsuarioSistema()
    {
        try
        {
            var usuarioObj = new UsuarioModel();

            usuarioObj.IdUsuario = "ACELERA";
            usuarioObj.IdUnidadeInst = "0";
            usuarioObj.IdInstituicao = 691;
            usuarioObj.NumCheckAlteracao = 0;
            usuarioObj.IdInstituicaoUsuario = 691;
            usuarioObj.DescNomeUsuario = "ACELERA PLENO";
            usuarioObj.DescEmail = "acelera@acelera.com";
            usuarioObj.BolHabilitadoUsuario = 1;
            usuarioObj.DescStatusUsuario = null;
            usuarioObj.BolVerificaNomeMaquina = 0;
            usuarioObj.CodCriadoPor = 1;
            usuarioObj.DataHoraCriacao = DateTime.Now;

            _dataContext.Add(usuarioObj);
            _dataContext.SaveChanges();

            var usuarioSistemaObj = new UsuarioSistemaModel();

            usuarioSistemaObj.IdUsuario = "ACELERA";
            usuarioSistemaObj.Login = "ACELERA";
            usuarioSistemaObj.SenhaHash = null;
            usuarioSistemaObj.SenhaSalt = null;
            usuarioSistemaObj.SecretKey = "PWWTMN3EZDVCOEY32ZJASLFYUDZGAH4T";
            usuarioSistemaObj.BolPrimeiroLogin = 1;
            usuarioSistemaObj.CodCriadoPor = 1;
            usuarioSistemaObj.DataHoraCriacao = DateTime.Now;

            var grupoAcessoObj1 = new GrupoAcessoModel();

            grupoAcessoObj1.DescGrupoAcesso = "Administrador";
            grupoAcessoObj1.CodCriadoPor = 1;
            grupoAcessoObj1.DataHoraCriacao = DateTime.Now;

            var grupoAcessoObj2 = new GrupoAcessoModel();

            grupoAcessoObj2.DescGrupoAcesso = "TI";
            grupoAcessoObj2.CodCriadoPor = 1;
            grupoAcessoObj2.DataHoraCriacao = DateTime.Now;

            var grupoAcessoObj3 = new GrupoAcessoModel();

            grupoAcessoObj3.DescGrupoAcesso = "Operacional";
            grupoAcessoObj3.CodCriadoPor = 1;
            grupoAcessoObj3.DataHoraCriacao = DateTime.Now;

            _dataContext.Add(usuarioSistemaObj);
            _dataContext.Add(grupoAcessoObj1);
            _dataContext.Add(grupoAcessoObj2);
            _dataContext.Add(grupoAcessoObj3);
            _dataContext.SaveChanges();

            var usuarioSistemaGrupoAcessoObj = new UsuarioSistemaGrupoAcessoModel();

            usuarioSistemaGrupoAcessoObj.IdUsuarioSistema = _dataContext.UsuarioSistema.Where(u => u.IdUsuario == usuarioSistemaObj.IdUsuario).First().IdUsuarioSistema;
            usuarioSistemaGrupoAcessoObj.IdGrupoAcesso = _dataContext.GrupoAcesso.Where(u => u.DescGrupoAcesso == grupoAcessoObj1.DescGrupoAcesso).First().IdGrupoAcesso;
            usuarioSistemaGrupoAcessoObj.CodCriadoPor = 1;
            usuarioSistemaGrupoAcessoObj.DataHoraCriacao = DateTime.Now;

            _dataContext.Add(usuarioSistemaGrupoAcessoObj);
            _dataContext.SaveChanges();

            return Ok("Usuário cadastrado com sucesso!");
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("UnidadeInstituicao")]
    public IActionResult InserirUnidadeInstituicao()
    {
        try
        {
            var unidadeInstList = new List<UnidadeInstituicaoModel>();

            var idUsuarioLogado = 1;//Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/UNIDADEINSTITUICAO.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 3; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 4].Value)))
                    {
                        var unidadeInstObj = new UnidadeInstituicaoModel();

                        unidadeInstObj.IdUnidadeInstituicao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 2].Value));
                        unidadeInstObj.IdInstituicao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 1].Value));
                        unidadeInstObj.Cnpj = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 3].Value));
                        unidadeInstObj.NomeUnidade = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 4].Value));
                        unidadeInstObj.SiglaUnidade = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 5].Value));
                        unidadeInstObj.DataCadastramento = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 6].Value));
                        unidadeInstObj.CodTipoInstituicao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 7].Value));
                        unidadeInstObj.CodTipoUnidade = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 8].Value));
                        unidadeInstObj.DataInicioSicoob = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 9].Value));
                        unidadeInstObj.DataFimSicoob = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 10].Value));
                        unidadeInstObj.NumCheckAlteracao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 12].Value));
                        unidadeInstObj.IdUnidadeInstResp = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 13].Value));
                        unidadeInstObj.CodSituacaoUnid = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 14].Value));
                        unidadeInstObj.NumSirc = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 15].Value));
                        unidadeInstObj.DescricaoEndInternet = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 16].Value));
                        unidadeInstObj.DataInicioUtilizacaoMarcaSicoob = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 17].Value));
                        unidadeInstObj.BolAtentimentoPublicoExterno = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 18].Value));
                        unidadeInstObj.NumInscricaoMunicipal = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 19].Value));
                        unidadeInstObj.NumNire = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 20].Value));
                        unidadeInstObj.IdInstituicaoIncorporadora = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 21].Value));
                        unidadeInstObj.DataIncorporacao = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 22].Value));
                        unidadeInstObj.BolUtilizaCompartilhamento = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 23].Value));
                        unidadeInstObj.DataInicioFuncionamento = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 24].Value));
                        unidadeInstObj.DataFimFuncionamento = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 25].Value));
                        unidadeInstObj.BolUtilizaSisbr = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 26].Value));
                        unidadeInstObj.DataInicioUtilizaSisbr = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 27].Value));
                        unidadeInstObj.DataFimUtilizaSisbr = _validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 28].Value));
                        unidadeInstObj.BolIsentoInscricaoMunicipal = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 29].Value));
                        unidadeInstObj.BolIsentoNire = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 30].Value));
                        unidadeInstObj.BolSinalizadoSicoob = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 31].Value));
                        unidadeInstObj.BolPaIncorporado = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 32].Value));
                        unidadeInstObj.DataHoraCarga = (DateTime)_validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 11].Value));
                        unidadeInstObj.CodCriadoPor = idUsuarioLogado;
                        unidadeInstObj.DataHoraCriacao = DateTime.Now;

                        unidadeInstList.Add(unidadeInstObj);
                    }
                }
            }

            _dataContext.AddRange(unidadeInstList);
            _dataContext.SaveChanges();

            return Ok(unidadeInstList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("TransportadoraValores")]
    public IActionResult InserirTransportadoraValores()
    {
        try
        {
            var transportadoraList = new List<TransportadoraValoresModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/Transportadoras de Valores.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 3; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 2].Value)))
                    {
                        var transportadoraObj = new TransportadoraValoresModel();

                        transportadoraObj.Cnpj = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 2].Value));
                        transportadoraObj.DescricaoTransportadora = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 3].Value));
                        transportadoraObj.CodCriadoPor = idUsuarioLogado;
                        transportadoraObj.DataHoraCriacao = DateTime.Now;

                        transportadoraList.Add(transportadoraObj);
                    }
                }
            }

            _dataContext.AddRange(transportadoraList);
            _dataContext.SaveChanges();

            return Ok(transportadoraList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("UnidadeInstituicaoTransportadoraValores")]
    public IActionResult InserirUnidadeInstTransportadoraValores()
    {
        try
        {
            var unidadeTransportadoraList = new List<UnidadeInstituicaoTransportadoraValoresModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/Transportadoras de Valores.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 3; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 4].Value)))
                    {
                        string cnpjTransportadora = excelWS.Cells[i, 2].Value.ToString();

                        var transportadoraObj = _dataContext.TransportadoraValores.Where(t => t.Cnpj.Contains(cnpjTransportadora)).FirstOrDefault();

                        if (transportadoraObj != null)
                        {
                            string[] unidadeInstList = excelWS.Cells[i, 4].Value.ToString().Split(',');

                            foreach (string pa in unidadeInstList)
                            {
                                var unidadeTransportadoraObj = new UnidadeInstituicaoTransportadoraValoresModel();

                                unidadeTransportadoraObj.IdTransportadoraValores = transportadoraObj.IdTransportadoraValores;
                                unidadeTransportadoraObj.IdUnidadeInst = pa;
                                unidadeTransportadoraObj.CodCriadoPor = idUsuarioLogado;
                                unidadeTransportadoraObj.DataHoraCriacao = DateTime.Now;

                                unidadeTransportadoraList.Add(unidadeTransportadoraObj);
                            }
                        }
                    }
                }
            }

            _dataContext.AddRange(unidadeTransportadoraList);
            _dataContext.SaveChanges();

            return Ok(unidadeTransportadoraList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("TipoTerminal")]
    public IActionResult InserirTipoTerminal()
    {
        try
        {
            var tipoTerminalList = new List<TipoTerminalModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/TIPOS_TERMINAL.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 2; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 3].Value)))
                    {
                        var tipoTerminalObj = new TipoTerminalModel();

                        tipoTerminalObj.IdTipoTerminal = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 2].Value));
                        tipoTerminalObj.IdUnidadeInst = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 1].Value));
                        tipoTerminalObj.DescricaoTipoTerminal = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 3].Value));
                        tipoTerminalObj.BolAcessoLiberado = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 6].Value));
                        tipoTerminalObj.NumCheckAlteracao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 7].Value));
                        tipoTerminalObj.LimiteSuperior = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 4].Value));
                        tipoTerminalObj.LimiteInferior = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 5].Value));
                        tipoTerminalObj.CodCriadoPor = idUsuarioLogado;
                        tipoTerminalObj.DataHoraCriacao = DateTime.Now;

                        tipoTerminalList.Add(tipoTerminalObj);
                    }
                }

                _dataContext.AddRange(tipoTerminalList);
                _dataContext.SaveChanges();

                return Ok(tipoTerminalList);

            }
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("TipoOperacao")]
    public IActionResult InserirTipoOperacao()
    {
        try
        {
            var tipoOperacaoList = new List<TipoOperacaoModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/Sensibilização de Saldos.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 2; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 3].Value)))
                    {
                        var tipoOperacaoObj = new TipoOperacaoModel();

                        tipoOperacaoObj.IdGrupoCaixa = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 1].Value));
                        tipoOperacaoObj.IdOperacaoCaixa = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 2].Value));
                        tipoOperacaoObj.Operacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 3].Value));
                        tipoOperacaoObj.DescricaoOperacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 4].Value));
                        tipoOperacaoObj.CodHistorico = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 5].Value));
                        tipoOperacaoObj.DescricaoHistorico = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 6].Value));
                        tipoOperacaoObj.Sensibilizacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 7].Value));
                        tipoOperacaoObj.CodCriadoPor = idUsuarioLogado;
                        tipoOperacaoObj.DataHoraCriacao = DateTime.Now;

                        tipoOperacaoList.Add(tipoOperacaoObj);
                    }
                }
            }

            _dataContext.AddRange(tipoOperacaoList);
            _dataContext.SaveChanges();

            return Ok(tipoOperacaoList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("SaldosIniciais")]
    public IActionResult InserirSaldosIniciais()
    {
        try
        {
            var operacaoList = new List<OperacaoModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/SALDOS INICIAIS 01.09.2022.xlsx");

            var tipoTerminalList = new List<TipoTerminalModel> {
            new TipoTerminalModel
            {
                IdTipoTerminal = 2,
                DescricaoTipoTerminal = "CAIXAS"
            },
            new TipoTerminalModel
            {
                IdTipoTerminal = 5,
                DescricaoTipoTerminal = "ATMS "
            },
            new TipoTerminalModel
            {
                IdTipoTerminal = 8,
                DescricaoTipoTerminal = "TESOUREIROS ELETETRÔNICOS"
            }
        };

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                for (int t = 0; t < excelPackage.Workbook.Worksheets.Count; t++)
                {
                    ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[t];

                    var tipoTerminal = tipoTerminalList.Where(t => t.DescricaoTipoTerminal == excelWS.Name).FirstOrDefault();

                    if (tipoTerminal != null)
                    {
                        int rowCount = excelWS.Dimension.Rows;
                        int columnCount = excelWS.Dimension.Columns;

                        for (int i = 3; i <= rowCount; i++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 6].Value)))
                            {
                                var operacaoObj = new OperacaoModel();

                                operacaoObj.IdTipoTerminal = tipoTerminal.IdTipoTerminal;
                                operacaoObj.IdUnidadeInst = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 7].Value));
                                operacaoObj.Operacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 1].Value));
                                operacaoObj.DescricaoOperacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 2].Value));
                                operacaoObj.CodHistorico = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 3].Value));
                                operacaoObj.DescricaoHistorico = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 4].Value));
                                operacaoObj.DataOperacao = (DateTime)_validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 5].Value));
                                operacaoObj.Terminal = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 6].Value));
                                operacaoObj.CodigoAut = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 8].Value));
                                operacaoObj.Valor = (decimal)_validar.VerificarDadosMonetario(Convert.ToString(excelWS.Cells[i, 9].Value));

                                var tipoOperacaoObj = _dataContext.TipoOperacao.Where(t => t.Operacao == operacaoObj.Operacao && t.CodHistorico == operacaoObj.CodHistorico).FirstOrDefault();

                                operacaoObj.IdTipoOperacao = tipoOperacaoObj.IdTipoOperacao;
                                operacaoObj.Sensibilizacao = tipoOperacaoObj.Sensibilizacao;
                                operacaoObj.CodCriadoPor = idUsuarioLogado;
                                operacaoObj.DataHoraCriacao = DateTime.Now;

                                operacaoList.Add(operacaoObj);
                            }
                        }
                    }
                }
            }

            _dataContext.AddRange(operacaoList);
            _dataContext.SaveChanges();

            return Ok(operacaoList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Usuario")]
    public IActionResult InserirUsuario()
    {
        try
        {
            var usuarioList = new List<UsuarioModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/Base de Usuários e Terminais Atuailzada.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 2; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 4].Value)))
                    {
                        var usuarioObj = new UsuarioModel();

                        usuarioObj.IdUsuario = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 4].Value));
                        usuarioObj.IdUnidadeInst = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 6].Value));
                        usuarioObj.IdInstituicao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 1].Value));
                        usuarioObj.NumCheckAlteracao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 3].Value));
                        usuarioObj.IdInstituicaoUsuario = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 5].Value));
                        usuarioObj.DescNomeUsuario = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 7].Value));
                        usuarioObj.DescEmail = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 11].Value));
                        usuarioObj.BolHabilitadoUsuario = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 8].Value));
                        usuarioObj.DescStatusUsuario = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 9].Value));
                        usuarioObj.BolVerificaNomeMaquina = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 10].Value));
                        usuarioObj.CodCriadoPor = idUsuarioLogado;
                        usuarioObj.DataHoraCriacao = DateTime.Now;

                        usuarioList.Add(usuarioObj);
                    }
                }
            }

            _dataContext.AddRange(usuarioList);
            _dataContext.SaveChanges();

            return Ok(usuarioList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Terminal")]
    public IActionResult InserirTerminal()
    {
        try
        {
            var terminalList = new List<TerminalModel>();

            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            FileInfo fileInfo = new FileInfo("Resources/TERMINAL.xlsx");

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet excelWS = excelPackage.Workbook.Worksheets[0];

                int rowCount = excelWS.Dimension.Rows;
                int columnCount = excelWS.Dimension.Columns;

                for (int i = 3; i <= rowCount + 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(excelWS.Cells[i, 8].Value)))
                    {
                        var terminalObj = new TerminalModel();

                        terminalObj.IdUnidadeInst = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 3].Value));
                        terminalObj.IdTipoTerminal = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 6].Value));
                        terminalObj.IdUsuario = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 8].Value));
                        terminalObj.IdUsuarioLiberacao = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 9].Value));
                        terminalObj.IdInstituicao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 1].Value));
                        terminalObj.IdProduto = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 2].Value));
                        terminalObj.DataProcessamento = (DateTime)_validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 4].Value));
                        terminalObj.NumTerminal = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 5].Value));
                        terminalObj.IdSituacaoTerminal = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 7].Value));
                        terminalObj.DescEstTrabalho = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 10].Value));
                        terminalObj.NumUltAutenticacao = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 11].Value));
                        terminalObj.NumLoteCco = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 12].Value));
                        terminalObj.MenorValorNota = (decimal)_validar.VerificarDadosMonetario(Convert.ToString(excelWS.Cells[i, 13].Value));
                        terminalObj.DataHoraLiberacao = (DateTime)_validar.VerificarDadosData(Convert.ToString(excelWS.Cells[i, 14].Value));
                        terminalObj.NumLoteCheque = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 15].Value));
                        terminalObj.NumUltSeqLancCco = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 16].Value));
                        terminalObj.NumUltRemessa = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 17].Value));
                        terminalObj.IdClienteCor = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 18].Value));
                        terminalObj.NumLoteDoc = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 19].Value));
                        terminalObj.NumUltSeqDoc = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 20].Value));
                        terminalObj.DescVersaoSo = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 21].Value));
                        terminalObj.DescMemoriaRam = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 22].Value));
                        terminalObj.DescEspacoDisco = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 23].Value));
                        terminalObj.DescPacoteServico = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 24].Value));
                        terminalObj.NumLoteDec = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 25].Value));
                        terminalObj.NumUltSeqDec = (int)_validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 26].Value));
                        terminalObj.ValorLimiteSaque = (decimal)_validar.VerificarDadosMonetario(Convert.ToString(excelWS.Cells[i, 27].Value));
                        terminalObj.ValorLimiteTerminal = (decimal)_validar.VerificarDadosMonetario(Convert.ToString(excelWS.Cells[i, 28].Value));
                        terminalObj.NumTesoureiro = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 29].Value));
                        terminalObj.NumIpTesoureiro = _validar.VerificarDadosTexto(Convert.ToString(excelWS.Cells[i, 30].Value));
                        terminalObj.CodTipoBalanceamento = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 31].Value));
                        terminalObj.NumTimeOutDispensador = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 32].Value));
                        terminalObj.CodLadoDepositario = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 33].Value));
                        terminalObj.NumPortaTesoureiro = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 34].Value));
                        terminalObj.NumCheckAlteracao = _validar.VerificarDadosInteiros(Convert.ToString(excelWS.Cells[i, 35].Value));
                        terminalObj.CodCriadoPor = idUsuarioLogado;
                        terminalObj.DataHoraCriacao = DateTime.Now;

                        terminalList.Add(terminalObj);
                    }
                }
            }

            _dataContext.AddRange(terminalList);
            _dataContext.SaveChanges();

            return Ok(terminalList);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Parametrizacao")]
    public IActionResult InserirParametrizacao()
    {
        try
        {
            var idUsuarioLogado = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var parametrizacaoList = new List<ParametrizacaoModel> {
            new ParametrizacaoModel{
                TipoCarga = "UNIDADEINSTITUICAO",
                CaminhoCarga = "processando/pa",
                IntervaloExecucao = 2,
                CodCriadoPor = idUsuarioLogado,
                DataHoraCriacao = DateTime.Now
            },
            new ParametrizacaoModel() {

                TipoCarga = "OPERACAO",
                CaminhoCarga = "processando/operacao",
                IntervaloExecucao = 2,
                CodCriadoPor = idUsuarioLogado,
                DataHoraCriacao = DateTime.Now
            },
            new ParametrizacaoModel{
                TipoCarga = "USUARIO",
                CaminhoCarga = "processando/usuario",
                IntervaloExecucao = 2,
                CodCriadoPor = idUsuarioLogado,
                DataHoraCriacao = DateTime.Now
            },
            new ParametrizacaoModel
            {
                TipoCarga = "TERMINAL",
                CaminhoCarga = "processando/terminal",
                IntervaloExecucao = 2,
                CodCriadoPor = idUsuarioLogado,
                DataHoraCriacao = DateTime.Now
            }
        };

            _dataContext.AddRange(parametrizacaoList);
            _dataContext.SaveChanges();

            return Ok("Parametrizações cadastradas com sucesso!");
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}