using GastosManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastosManagement.Infrastructure.Persistence.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                   .IsRequired()
                   .HasMaxLength(120);

            // Evitar categorías duplicadas por nombre
            builder.HasIndex(c => c.Nombre)
                   .IsUnique();

            // Relación 1 Categoria -> N Gastos
            builder.HasMany(c => c.Gastos)
                   .WithOne(g => g.Categoria!)
                   .HasForeignKey(g => g.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);
            // Restrict evita borrar una categoría si tiene gastos (buena práctica en finanzas)
        }
    }
}