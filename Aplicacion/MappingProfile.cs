using Aplicacion.Comentarios.DTOcomentarios;
using Aplicacion.CursoInstructores.DTOcursoInstructores;
using Aplicacion.Cursos.DTOcursos;
using Aplicacion.Instructores.DTOinstructores;
using Aplicacion.Precios.DTOprecios;
using AutoMapper;
using Dominio;
using System.Linq;

namespace Aplicacion
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDTO>()
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLink.Select(a => a.Instructor).ToList()))
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.Precio));
            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Instructor, InstructorDTO>()
                .ForMember(x => x.Cursos, y => y.MapFrom(z => z.CursosLink.Select(a => a.Curso).ToList()));
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();
        }
    }
}
