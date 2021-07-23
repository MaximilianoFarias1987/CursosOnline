using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    public class InsertarComentario
    {
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
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
                var comentario = new Comentario
                {
                    Id = Guid.NewGuid(),
                    Alumno = request.Alumno,
                    Puntaje = request.Puntaje,
                    ComentarioTexto = request.Comentario,
                    CursoId = request.CursoId,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Comentarios.Add(comentario);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el Comentario");
            }
        }
    }
}
