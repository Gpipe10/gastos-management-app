using System;
using System.Collections.Generic;
using System.Text;

namespace GastosManagement.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Navegación: 1 Categoria -> N Gastos
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}