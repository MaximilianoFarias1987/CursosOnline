using Aplicacion.Instructores.DTOinstructores;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<InstructorDTO>>
        {
            //Esto va a devolver una lista de cursos
        }

        public class Manejador : IRequestHandler<Ejecuta, List<InstructorDTO>>
        {
            private readonly CursosContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<InstructorDTO>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _context.Instructores
                    .Include(x => x.CursosLink)
                    .ThenInclude(x => x.Curso)
                    .ThenInclude(x => x.Precio)
                    .ToListAsync();

                var InstructorDTO = _mapper.Map<List<Instructor>, List<InstructorDTO>>(instructor);

                return InstructorDTO;
            }
        }
    }
}

