using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Instructor
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public ICollection<CursoInstructor> CursosLink { get; set; }
    }
}
