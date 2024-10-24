using AceleraPlenoProjetoFinal.Api.Data;
using AceleraPlenoProjetoFinal.Api.Models;
using AceleraPlenoProjetoFinal.Api.Repositories.Interfaces;
using AceleraPlenoProjetoFinal.Api.Validations;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.Net;

namespace AceleraPlenoProjetoFinal.Api.Repositories;

public class CargaRepository : ICargaRepository
{
    private readonly DataContext _dataContext;
    private readonly ValidarDadosCarga _validar;

    public CargaRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _validar = new ValidarDadosCarga();
    }

    public bool InserirUnidadeInstituicao(string filePath)
    {
        try
        {
            InserirLog($"Arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} iniciou processo de carga!", "AVISO");

            var unidadeInstList = new List<UnidadeInstituicaoModel>();

            using (var webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(filePath);

                using (var memoryStream = new MemoryStream(data))
                {
                    using (var excelPackage = new ExcelPackage(memoryStream))
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
                                unidadeInstObj.CodCriadoPor = 1;
                                unidadeInstObj.DataHoraCriacao = DateTime.Now;

                                unidadeInstList.Add(unidadeInstObj);

                                _dataContext.Add(unidadeInstObj);
                            }
                        }
                    }
                }
            }

            _dataContext.SaveChanges();

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} processado com sucesso.", "OK");

            return true;
        }

        catch (Exception ex)
        {
            _dataContext.RemoveRange(_dataContext.UnidadeInstituicao.Local);

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} não foi realizado com sucesso. Será feito um novo processamento de carga no próximo agendamento.", "ERRO");

            return false;
        }
    }

    public bool InserirOperacao(string filePath)
    {
        try
        {
            InserirLog($"Arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} iniciou processo de carga!", "AVISO");

            var operacaoList = new List<OperacaoModel>();

            using (var httpClient = new HttpClient())
            {
                string data = httpClient.GetStringAsync(filePath).Result;
                string[] rows = data.Split('\n');
                string[][] dataRows = rows.Select(r => r.Split(',')).ToArray();

                int rowCount = dataRows.Count();

                for (int i = 0; i < rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dataRows[i][5])))
                    {
                        var operacaoObj = new OperacaoModel();

                        operacaoObj.IdTipoTerminal = 2;

                        var agPaTerm = _validar.VerificarDadosTexto(Convert.ToString(dataRows[i][5])).Split("/");
                        int idGrupoCaixa = (int)_validar.VerificarDadosInteiros(Convert.ToString(dataRows[i][9]));
                        int idOperacaoCaixa = (int)_validar.VerificarDadosInteiros(Convert.ToString(dataRows[i][10]));
                        int codHistorico = (int)_validar.VerificarDadosInteiros(Convert.ToString(dataRows[i][13]));

                        operacaoObj.IdUnidadeInst = Convert.ToString(Convert.ToInt16(agPaTerm[1]));

                        var tipoOperacaoResult = _dataContext.TipoOperacao.Where(t => t.IdGrupoCaixa == idGrupoCaixa && t.IdOperacaoCaixa == idOperacaoCaixa && t.CodHistorico == codHistorico && t.DataHoraInativo == null).FirstOrDefault();

                        operacaoObj.Operacao = tipoOperacaoResult.Operacao;
                        operacaoObj.DescricaoOperacao = tipoOperacaoResult.DescricaoOperacao;
                        operacaoObj.CodHistorico = codHistorico;
                        operacaoObj.DescricaoHistorico = tipoOperacaoResult.DescricaoHistorico;
                        operacaoObj.DataOperacao = (DateTime)_validar.VerificarDadosData(Convert.ToString(dataRows[i][2]));
                        operacaoObj.Terminal = _validar.VerificarDadosTexto(Convert.ToString(dataRows[i][5]));
                        operacaoObj.CodigoAut = null;
                        operacaoObj.Valor = (decimal)_validar.VerificarDadosMonetario(Convert.ToString(dataRows[i][17]));
                        operacaoObj.IdTipoOperacao = tipoOperacaoResult.IdTipoOperacao;
                        operacaoObj.Sensibilizacao = tipoOperacaoResult.Sensibilizacao;
                        operacaoObj.CodCriadoPor = 1;
                        operacaoObj.DataHoraCriacao = DateTime.Now;

                        var operacaoData = _dataContext.Operacao.Where(
                            o => o.IdTipoOperacao == operacaoObj.IdTipoOperacao &&
                            o.IdTipoTerminal == operacaoObj.IdTipoTerminal &&
                            o.IdUnidadeInst == operacaoObj.IdUnidadeInst &&
                            o.Operacao == operacaoObj.Operacao &&
                            o.CodHistorico == operacaoObj.CodHistorico &&
                            o.DataOperacao == operacaoObj.DataOperacao &&
                            o.Terminal == operacaoObj.Terminal &&
                            o.Valor == operacaoObj.Valor &&
                            o.DataHoraInativo == null
                        ).FirstOrDefault();

                        if (operacaoData == null)
                        {
                            operacaoList.Add(operacaoObj);
                            _dataContext.Add(operacaoObj);
                        }
                    }
                }
            }

            _dataContext.SaveChanges();

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} processado com sucesso.", "OK");

            return true;
        }

        catch (Exception ex)
        {
            _dataContext.RemoveRange(_dataContext.Operacao.Local);

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} não foi realizado com sucesso. Será feito um novo processamento de carga no próximo agendamento.", "ERRO");

            return false;
        }
    }

    public bool InserirUsuario(string filePath)
    {
        try
        {
            InserirLog($"Arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} iniciou processo de carga!", "AVISO");

            var usuarioList = new List<UsuarioModel>();

            using (var webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(filePath);

                using (var memoryStream = new MemoryStream(data))
                {
                    using (var excelPackage = new ExcelPackage(memoryStream))
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
                                usuarioObj.CodCriadoPor = 1;
                                usuarioObj.DataHoraCriacao = DateTime.Now;

                                usuarioList.Add(usuarioObj);

                                _dataContext.Add(usuarioObj);
                            }
                        }
                    }
                }
            }

            _dataContext.SaveChanges();

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} processado com sucesso.", "OK");

            return true;
        }

        catch (Exception ex)
        {
            _dataContext.RemoveRange(_dataContext.Usuario.Local);

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} não foi realizado com sucesso. Será feito um novo processamento de carga no próximo agendamento.", "ERRO");

            return false;
        }
    }

    public bool InserirTerminal(string filePath)
    {
        try
        {
            InserirLog($"Arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} iniciou processo de carga!", "AVISO");

            var terminalList = new List<TerminalModel>();

            using (var webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(filePath);

                using (var memoryStream = new MemoryStream(data))
                {
                    using (var excelPackage = new ExcelPackage(memoryStream))
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
                                terminalObj.CodCriadoPor = 1;
                                terminalObj.DataHoraCriacao = DateTime.Now;

                                terminalList.Add(terminalObj);

                                _dataContext.Add(terminalObj);
                            }
                        }
                    }
                }
            }

            _dataContext.SaveChanges();

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} processado com sucesso.", "OK");

            return true;
        }

        catch (Exception ex)
        {
            _dataContext.RemoveRange(_dataContext.Terminal.Local);

            InserirLog($"Carga do arquivo {Regex.Replace(Path.GetFileNameWithoutExtension(filePath), @"^\d+_", "")} não foi realizado com sucesso. Será feito um novo processamento de carga no próximo agendamento.", "ERRO");

            return false;
        }
    }

    private void InserirLog(string descricao, string status)
    {
        _dataContext.Log.Add(new LogModel
        {
            DescLog = descricao,
            Status = status,
            DataHoraLog = DateTime.Now
        });

        _dataContext.SaveChanges();
    }
}