using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Comentarios.DTOcomentarios
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public Guid CursoId { get; set; }
    }
}
