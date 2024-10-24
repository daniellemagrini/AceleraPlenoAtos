using ConsultaNumerarios.Api.Models;
using ConsultaNumerarios.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultaNumerarios.Data;

public class DataContext : DbContext
{
    public DataContext() { }

    public DataContext(DbContextOptions<DataContext> opt) : base(opt) { }

    public DbSet<TransportadoraValoresModel> TransportadoraValores { get; set; }

    public DbSet<UnidadeInstituicaoModel> UnidadeInstituicao { get; set; }
    public DbSet<UsuarioModel> Usuario { get; set; }

    public DbSet<UnidadeInstituicaoTransportadoraValoresModel> UnidadeInstituicaoTransportadoraValores { get; set; }

    public DbSet<TipoTerminalModel> TipoTerminal { get; set; }
    public DbSet<TerminalModel> Terminal { get; set; }

    public DbSet<TipoOperacaoModel> TipoOperacao { get; set; }

    public DbSet<OperacaoModel> Operacao { get; set; }
}