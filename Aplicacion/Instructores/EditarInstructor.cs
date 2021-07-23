using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class EditarInstructor
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Debe ingresar un titulo");
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
                var instructor = await _context.Instructores.FindAsync(request.Id);
                if (instructor == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el instructor" });
                }

                instructor.Nombre = request.Nombre ?? instructor.Nombre; //si no le modifico el titulo que le deje el que ya tenia
                instructor.Apellido = request.Apellido ?? instructor.Apellido;
                instructor.Grado = request.Grado ?? instructor.Grado;

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el Instructor");
            }
        }
    }
}
