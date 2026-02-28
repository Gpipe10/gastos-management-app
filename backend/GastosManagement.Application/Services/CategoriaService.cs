using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.DTOs.Responses;
using GastosManagement.Application.Interfaces;
using GastosManagement.Domain.Entities;

namespace GastosManagement.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaService(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
        }

        // ✅ CAMBIO: ahora devuelve IEnumerable para coincidir con la interfaz
        public async Task<IEnumerable<CategoriaResponse>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();

            return categorias.Select(c => new CategoriaResponse
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();
        }

        // ✅ NUEVO
        public async Task<CategoriaResponse?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null) return null;

            return new CategoriaResponse
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<CategoriaResponse> CreateAsync(CategoriaCreateRequest request)
        {
            var nombre = (request.Nombre ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            // Evitar duplicados por nombre
            var existing = await _categoriaRepository.GetByNameAsync(nombre);
            if (existing != null)
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");

            var categoria = new Categoria
            {
                Nombre = nombre
            };

            await _categoriaRepository.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            return new CategoriaResponse
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        // ✅ NUEVO
        public async Task DeleteAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException("Categoría no encontrada.");

            // Recomendado: evitar borrar si tiene gastos asociados
            var tieneGastos = await _categoriaRepository.HasGastosAsync(id);
            if (tieneGastos)
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene gastos asociados.");

            await _categoriaRepository.DeleteAsync(categoria);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}