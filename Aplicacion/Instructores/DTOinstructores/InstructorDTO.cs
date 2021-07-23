using Aplicacion.Cursos.DTOcursos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Instructores.DTOinstructores
{
    public class InstructorDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public ICollection<CursoDTO> Cursos { get; set; }
    }
}
