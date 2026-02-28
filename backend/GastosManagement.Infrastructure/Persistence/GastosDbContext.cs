using GastosManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GastosManagement.Infrastructure.Persistence
{
    public class GastosDbContext : DbContext
    {
        public GastosDbContext(DbContextOptions<GastosDbContext> options) : base(options)
        {
        }

        public DbSet<Gasto> Gastos => Set<Gasto>();
        public DbSet<Categoria> Categorias => Set<Categoria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica configuraciones Fluent API (separadas por entidad)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GastosDbContext).Assembly);
        }
    }
}