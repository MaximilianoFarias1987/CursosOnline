using Aplicacion.Instructores.DTOinstructores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos.DTOcursos
{
    public class CursoDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public ICollection<InstructorDTO> Instructores { get; set; }
    }
}
