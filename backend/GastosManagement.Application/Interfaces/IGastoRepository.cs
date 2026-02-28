using GastosManagement.Domain.Entities;

namespace GastosManagement.Application.Interfaces
{
    public interface IGastoRepository
    {
        Task<List<Gasto>> GetAllAsync();
        Task<Gasto?> GetByIdAsync(int id);
        Task AddAsync(Gasto gasto);
        void Update(Gasto gasto);
        void Delete(Gasto gasto);
        Task<bool> ExistsDuplicateAsync(string descripcionNormalizada, decimal monto, int categoriaId, DateTime fecha);


    }
}