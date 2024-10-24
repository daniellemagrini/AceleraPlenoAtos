using AceleraPlenoProjetoFinal.Api.Models;
using Microsoft.EntityFrameworkCore;
using Acelera.API.AuthOriginal.Model;

namespace Acelera.API.AuthOriginal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioSistema> Login { get; set; }
        public DbSet<UsuarioSistemaGrupoAcesso> UsuarioGrupoAcesso { get; set; }
        public DbSet<GrupoAcesso> GrupoAcesso { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
    }
}
