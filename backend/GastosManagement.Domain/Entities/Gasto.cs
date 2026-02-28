using System;
using System.Collections.Generic;
using System.Text;



namespace GastosManagement.Domain.Entities
{
    public class Gasto
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        // Importante: el tipo decimal es el correcto para dinero.
        public decimal Monto { get; set; }

        public DateTime FechaHora { get; set; }

        // FK
        public int CategoriaId { get; set; }

        // Navegación: N Gastos -> 1 Categoria
        public Categoria? Categoria { get; set; }
    }
}