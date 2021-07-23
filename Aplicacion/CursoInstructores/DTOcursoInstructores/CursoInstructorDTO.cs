using System;

namespace Aplicacion.CursoInstructores.DTOcursoInstructores
{
    class CursoInstructorDTO
    {
        public Guid CursoId { get; set; }
        public Guid InstructorId { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
