using System;
using System.Collections.Generic;
using System.Text;

using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.DTOs.Responses;

namespace GastosManagement.Application.Interfaces
{
    public interface IGastoService
    {
        Task<GastosResumenResponse> GetAllAsync();
        Task<GastoResponse> GetByIdAsync(int id);
        Task<GastoResponse> CreateAsync(GastoCreateRequest request);
        Task<GastoResponse> UpdateAsync(int id, GastoUpdateRequest request);
        Task DeleteAsync(int id);
    }
}