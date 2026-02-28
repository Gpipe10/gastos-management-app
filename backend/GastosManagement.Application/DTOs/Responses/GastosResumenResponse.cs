using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Application.DTOs.Responses
{
    public class GastosResumenResponse
    {
        public decimal Total { get; set; }
        public List<GastoResponse> Gastos { get; set; } = new();
        public int Registros { get; set; }
    }
}