using Microsoft.EntityFrameworkCore;
using Acelera.API.ParamLog.Model;

namespace Acelera.API.ParamLog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Parametrizacao> Parametrizacoes { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
