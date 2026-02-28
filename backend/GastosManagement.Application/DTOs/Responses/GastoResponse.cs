using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Application.DTOs.Responses
{
    public class GastoResponse
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }


        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}