using Aplicacion.Instructores.DTOinstructores;
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

namespace Aplicacion.Instructores
{
    public class ConsultaInstructorId
    {
        public class Ejecuta : IRequest<InstructorDTO>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, InstructorDTO>
        {
            private readonly CursosContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<InstructorDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _context.Instructores
                    .Include(x => x.CursosLink)
                    .ThenInclude(x => x.Curso)
                    .ThenInclude(x => x.Precio)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (instructor == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el Instructor" });
                }
                var instructorDTO = _mapper.Map<Instructor, InstructorDTO>(instructor);
                return instructorDTO;
            }
        }
    }
}
