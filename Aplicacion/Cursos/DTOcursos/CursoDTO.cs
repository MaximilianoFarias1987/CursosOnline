using Aplicacion.Comentarios.DTOcomentarios;
using Aplicacion.Instructores.DTOinstructores;
using Aplicacion.Precios.DTOprecios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aplicacion.Cursos.DTOcursos
{
    public class CursoDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public ICollection<InstructorDTO> Instructores { get; set; }
        public PrecioDTO Precio { get; set; }
        public ICollection<ComentarioDTO> Comentarios { get; set; }
    }
}
