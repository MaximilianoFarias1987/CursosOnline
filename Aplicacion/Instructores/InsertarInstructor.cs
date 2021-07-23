using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class InsertarInstructor
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Grado { get; set; }
            public byte[] FotoPerfil { get; set; }
        }


        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosContext _context;

            public Manejador(CursosContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _cursoId = Guid.NewGuid();
                var instructor = new Instructor
                {
                    Id = _cursoId,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Grado = request.Grado,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Instructores.Add(instructor);

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el Instructor");
            }
        }
    }
}
