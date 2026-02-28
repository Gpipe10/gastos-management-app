using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Application.DTOs.Requests
{
    public class GastoCreateRequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int CategoriaId { get; set; }
    }
}