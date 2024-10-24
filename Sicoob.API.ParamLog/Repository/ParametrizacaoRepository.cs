using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Acelera.API.ParamLog.Data;
using Acelera.API.ParamLog.DTO;
using Acelera.API.ParamLog.Model;
using Acelera.API.ParamLog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Acelera.API.ParamLog.Repository
{
    public class ParametrizacaoRepository : IParametrizacaoRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ParametrizacaoRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Método que busca todas as parametrizações cadastradas no sistema.
        /// </summary> 
        public async Task<List<Parametrizacao>> GetAllParametrizacao()
        {
            try
            {
                return await _context.Parametrizacoes.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível carregar a lista de Parametrizações. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que busca todas as parametrizações cadastradas no sistema.
        /// </summary> 
        public async Task<ActionResult<Parametrizacao>> GetParametrizacaoByID(int id)
        {
            try
            {
                return await _context.Parametrizacoes.SingleOrDefaultAsync(x => x.IDPARAMETRIZACAO == id);
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível encontrar a parametrização solicitada. Tente novamente mais tarde!");
            }
        }

        /// <summary>
        /// Método que atualiza a parametrização.
        /// </summary> 
        public async Task<bool> UpdateParametrizacao(ParametrizacaoDTO paramDTO, int loginAlteradoPor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var param = await _context.Parametrizacoes.SingleOrDefaultAsync(u => u.IDPARAMETRIZACAO == paramDTO.IDPARAMETRIZACAO);
                if (param != null)
                {
                    if(param.CODINATIVOPOR == null)
                    {
                        _mapper.Map(paramDTO, param);

                        param.CAMINHOCARGA = paramDTO.CAMINHOCARGA;
                        param.INTERVALOEXECUCAO = paramDTO.INTERVALOEXECUCAO;
                        param.CODALTERADOPOR = loginAlteradoPor;
                        param.DATAHORAALTERACAO = DateTime.Now;

                        _context.Parametrizacoes.Update(param);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    else throw new Exception("Parametrização inativa. Não pode ser alterada.");
                }
                else throw new Exception("Parametrização não existente.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Método que inativa a parametrização.
        /// </summary> 
        public async Task<bool> InativarParametrizacao(int id, int loginInativoPor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var param = await _context.Parametrizacoes.SingleOrDefaultAsync(u => u.IDPARAMETRIZACAO == id);
                if (param != null)
                {
                    if (param.CODINATIVOPOR == null)
                    {
                        param.CODINATIVOPOR = loginInativoPor;
                        param.DATAHORAINATIVO = DateTime.Now;

                        _context.Parametrizacoes.Update(param);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                    else throw new Exception("Parametrização já está inativa. nãopode inativar novamente");                   
                }
                else throw new Exception("Parametrização não existente."); 
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Método para verificar se a parametrização está inativa.
        /// </summary>
        public async Task<bool> VerificaParametrizacaoInativa(int idParam)
        {
            var resposta = false;

            try
            {
                var param = await _context.Parametrizacoes.SingleOrDefaultAsync(x => x.IDPARAMETRIZACAO == idParam);
                if (param != null)
                {
                    if (param.CODINATIVOPOR != null)
                    {
                        resposta = true;
                    }
                }
                else
                {
                    throw new Exception("Parametrização não encontrada!");
                }

                return resposta;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
