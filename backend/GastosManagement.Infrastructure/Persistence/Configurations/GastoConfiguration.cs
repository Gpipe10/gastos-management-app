using GastosManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastosManagement.Infrastructure.Persistence.Configurations
{
    public class GastoConfiguration : IEntityTypeConfiguration<Gasto>
    {
        public void Configure(EntityTypeBuilder<Gasto> builder)
        {
            builder.ToTable("Gastos");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Descripcion)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(g => g.Monto)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(g => g.FechaHora)
                   .IsRequired()
                   .HasColumnType("datetime2");

            builder.Property(g => g.CategoriaId)
                   .IsRequired();

            // Extra: índice para consultas (ordenar/filtrar por fecha)
            builder.HasIndex(g => g.FechaHora);

            // Extra: evitar doble registro (una regla razonable)
            builder.HasIndex(g => new { g.Descripcion, g.Monto, g.FechaHora })
                   .IsUnique();
        }
    }
}