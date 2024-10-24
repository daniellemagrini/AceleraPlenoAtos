using AceleraPlenoProjetoFinal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AceleraPlenoProjetoFinal.Api.Data;

public class DataContext : DbContext
{
    public DataContext() { }

    public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }

    public DbSet<TransportadoraValoresModel> TransportadoraValores { get; set; }

    public DbSet<UnidadeInstituicaoModel> UnidadeInstituicao { get; set; }

    public DbSet<UnidadeInstituicaoTransportadoraValoresModel> UnidadeInstituicaoTransportadoraValores { get; set; }

    public DbSet<TipoTerminalModel> TipoTerminal  { get; set; }

    public DbSet<TipoOperacaoModel> TipoOperacao { get; set; }

    public DbSet<OperacaoModel> Operacao { get; set; }
    
    public DbSet<UsuarioModel> Usuario { get; set; }

    public DbSet<TerminalModel> Terminal { get; set; }

    public DbSet<UsuarioSistemaModel> UsuarioSistema { get; set; }

    public DbSet<GrupoAcessoModel> GrupoAcesso { get; set; }

    public DbSet<UsuarioSistemaGrupoAcessoModel> UsuarioSistemaGrupoAcesso { get; set; }

    public DbSet<ParametrizacaoModel> Parametrizacao { get; set; }

    public DbSet<CargaModel> Carga { get; set; }

    public DbSet<LogModel> Log { get; set; }
}