using GastosManagement.Application.Interfaces;
using GastosManagement.Domain.Entities;
using GastosManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GastosManagement.Infrastructure.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly GastosDbContext _context;

        public GastoRepository(GastosDbContext context)
        {
            _context = context;
        }

        public async Task<List<Gasto>> GetAllAsync()
        {
            return await _context.Gastos
                .AsNoTracking()
                .Include(g => g.Categoria)
                .OrderByDescending(g => g.FechaHora)
                .ToListAsync();
        }

        public async Task<Gasto?> GetByIdAsync(int id)
        {
            return await _context.Gastos
                .AsNoTracking()
                .Include(g => g.Categoria)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Gasto gasto)
        {
            await _context.Gastos.AddAsync(gasto);
        }

        public void Update(Gasto gasto)
        {
            _context.Gastos.Update(gasto);
        }

        public void Delete(Gasto gasto)
        {
            _context.Gastos.Remove(gasto);
        }

        public async Task<bool> ExistsDuplicateAsync(string descripcionNormalizada, decimal monto, int categoriaId, DateTime fecha)
        {
            var dayStart = fecha.Date;
            var dayEnd = dayStart.AddDays(1);

            return await _context.Gastos.AnyAsync(g =>
                g.CategoriaId == categoriaId &&
                g.Monto == monto &&
                g.FechaHora >= dayStart && g.FechaHora < dayEnd &&
                g.Descripcion.ToLower() == descripcionNormalizada
            );
        }
    }
}