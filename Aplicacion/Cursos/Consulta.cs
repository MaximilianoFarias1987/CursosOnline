using Aplicacion.Cursos.DTOcursos;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>>
        {
            
        }

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Cursos
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.Precio)
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();


                var cursosDTO = _mapper.Map<List<Curso>, List<CursoDTO>>(cursos);

                return cursosDTO;



            }
        }
    }
}
