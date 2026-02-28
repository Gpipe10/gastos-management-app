using GastosManagement.Application.Interfaces;
using GastosManagement.Domain.Entities;
using GastosManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GastosManagement.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly GastosDbContext _context;

        public CategoriaRepository(GastosDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Categoria?> GetByNameAsync(string nombre)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        // ✅ NUEVO
        public Task DeleteAsync(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            return Task.CompletedTask;
        }

        // ✅ NUEVO
        public async Task<bool> HasGastosAsync(int categoriaId)
        {
            return await _context.Gastos
                .AnyAsync(g => g.CategoriaId == categoriaId);
        }
    }
}