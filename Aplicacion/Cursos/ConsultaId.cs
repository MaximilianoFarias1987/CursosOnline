using Aplicacion.Cursos.DTOcursos;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoId : IRequest<CursoDTO>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoId, CursoDTO>
        {
            private readonly CursosContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CursoDTO> Handle(CursoId request, CancellationToken cancellationToken)
            {
                var curso = await _context.Cursos
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.Precio)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                
                if (curso == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }
                var cursoDTO = _mapper.Map<Curso, CursoDTO>(curso);
                return cursoDTO;
            }
        }
    }
}
