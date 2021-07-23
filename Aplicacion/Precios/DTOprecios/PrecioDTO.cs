using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Precios.DTOprecios
{
    public class PrecioDTO
    {
        public Guid Id { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
    }
}
