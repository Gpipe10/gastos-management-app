using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Application.DTOs.Responses
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}