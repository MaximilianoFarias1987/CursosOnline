using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class InsertarCurso
    {
        public class Ejecuta : IRequest
        {
            //[Required(ErrorMessage ="Debe ingresar un titulo")]
            public string Titulo { get; set; }
            //[Required(ErrorMessage = "Debe ingresar una Descripción")]
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            //Agrego lo siguiente para representar los id  de los instructores
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }

        }


        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
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
                var curso = new Curso
                {
                    Id = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Cursos.Add(curso);

                //Ahora agrego a CursoInstructores

                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor {
                            CursoId = curso.Id,
                            InstructorId = id
                        };
                        _context.CursoInstructores.Add(cursoInstructor);
                    }
                }


                //Agrego un precio al curso
                var precio = new Precio { 
                    CursoId = curso.Id,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    Id = Guid.NewGuid()
                };
                _context.Precios.Add(precio);

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}
