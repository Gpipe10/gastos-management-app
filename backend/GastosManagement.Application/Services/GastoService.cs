using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.DTOs.Responses;
using GastosManagement.Application.Interfaces;
using GastosManagement.Domain.Entities;

namespace GastosManagement.Application.Services
{
    public class GastoService : IGastoService
    {
        private readonly IGastoRepository _gastoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GastoService(
            IGastoRepository gastoRepository,
            ICategoriaRepository categoriaRepository,
            IUnitOfWork unitOfWork)
        {
            _gastoRepository = gastoRepository;
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GastosResumenResponse> GetAllAsync()
        {
            var gastos = await _gastoRepository.GetAllAsync();

            var response = gastos.Select(g => new GastoResponse
            {
                Id = g.Id,
                Descripcion = g.Descripcion,
                Monto = g.Monto,
                FechaHora = g.FechaHora,
                CategoriaId = g.CategoriaId,
                CategoriaNombre = g.Categoria?.Nombre ?? string.Empty
            }).ToList();

            return new GastosResumenResponse
            {
                Total = response.Sum(x => x.Monto),
                Registros = response.Count,      // ✅ NUEVO
                Gastos = response
            };
        }

        public async Task<GastoResponse> GetByIdAsync(int id)
        {
            var gasto = await _gastoRepository.GetByIdAsync(id);
            if (gasto == null)
                throw new KeyNotFoundException("Gasto no encontrado.");

            return new GastoResponse
            {
                Id = gasto.Id,
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                FechaHora = gasto.FechaHora,
                CategoriaId = gasto.CategoriaId,
                CategoriaNombre = gasto.Categoria?.Nombre ?? string.Empty
            };
        }

        public async Task<GastoResponse> CreateAsync(GastoCreateRequest request)
        {
            var descripcion = (request.Descripcion ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción es obligatoria.");

            if (request.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor que 0.");

            // ✅ Validar categoría
            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new ArgumentException("La categoría no existe.");

            // ✅ Evitar doble registro (mismo día)
            var now = DateTime.Now;
            var descNorm = descripcion.ToLower();

            var existsDuplicate = await _gastoRepository.ExistsDuplicateAsync(descNorm, request.Monto, request.CategoriaId, now);
            if (existsDuplicate)
                throw new InvalidOperationException("Este gasto ya fue registrado hoy (posible duplicado).");

            // ✅ Fecha la define backend (fuente de verdad)
            var gasto = new Gasto
            {
                Descripcion = descripcion,
                Monto = request.Monto,
                FechaHora = now,
                CategoriaId = request.CategoriaId
            };

            await _gastoRepository.AddAsync(gasto);
            await _unitOfWork.SaveChangesAsync();

            return new GastoResponse
            {
                Id = gasto.Id,
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                FechaHora = gasto.FechaHora,
                CategoriaId = gasto.CategoriaId,
                CategoriaNombre = categoria.Nombre
            };
        }

        public async Task<GastoResponse> UpdateAsync(int id, GastoUpdateRequest request)
        {
            var gasto = await _gastoRepository.GetByIdAsync(id);
            if (gasto == null)
                throw new KeyNotFoundException("Gasto no encontrado.");

            var descripcion = (request.Descripcion ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción es obligatoria.");

            if (request.Monto <= 0)
                throw new ArgumentException("El monto debe ser mayor que 0.");

            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new ArgumentException("La categoría no existe.");

            // ✅ Actualizamos solo campos editables
            gasto.Descripcion = descripcion;
            gasto.Monto = request.Monto;
            gasto.CategoriaId = request.CategoriaId;

            // ✅ NO tocamos FechaHora en update
            _gastoRepository.Update(gasto);
            await _unitOfWork.SaveChangesAsync();

            return new GastoResponse
            {
                Id = gasto.Id,
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                FechaHora = gasto.FechaHora,
                CategoriaId = gasto.CategoriaId,
                CategoriaNombre = categoria.Nombre
            };
        }

        public async Task DeleteAsync(int id)
        {
            var gasto = await _gastoRepository.GetByIdAsync(id);
            if (gasto == null)
                throw new KeyNotFoundException("Gasto no encontrado.");

            _gastoRepository.Delete(gasto);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}