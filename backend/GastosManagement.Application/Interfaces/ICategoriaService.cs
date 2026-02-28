using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.DTOs.Responses;

namespace GastosManagement.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponse>> GetAllAsync();
        Task<CategoriaResponse?> GetByIdAsync(int id);
        Task<CategoriaResponse> CreateAsync(CategoriaCreateRequest request);
        Task DeleteAsync(int id);
    }
}