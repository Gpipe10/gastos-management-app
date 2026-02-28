using GastosManagement.Domain.Entities;

namespace GastosManagement.Application.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> GetAllAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<Categoria?> GetByNameAsync(string nombre);
        Task AddAsync(Categoria categoria);
        Task DeleteAsync(Categoria categoria);
        Task<bool> HasGastosAsync(int categoriaId);
    }
}