using ConsultaNumerarios.Data;
using ConsultaNumerarios.Dto;
using ConsultaNumerarios.Interfaces;

namespace ConsultaNumerarios.Repository
{
    public class UnidadeInstituicaoRepository : IUnidadeInstituicao
    {
        private readonly DataContext _context;
        public UnidadeInstituicaoRepository(DataContext context)
        {
            _context = context;
        }

        public List<Pa> GetUnidadeInstituicao()
        {
            List<Pa> retornoPa = new List<Pa>();
            var unidades = _context.UnidadeInstituicao
                .Where(pa => pa.Cnpj != null)
                .ToList();

            unidades.ForEach(unidade =>
            {
                Pa paIteracao = new Pa()
                {
                    IdPa = unidade.IdUnidadeInstituicao,
                    PaName = unidade.NomeUnidade,
                };

                retornoPa.Add(paIteracao);
            });
            return retornoPa;

            //return [.. _context.UnidadeInstituicao.Where(pa => pa.Cnpj != null)];
        }
    }
}
