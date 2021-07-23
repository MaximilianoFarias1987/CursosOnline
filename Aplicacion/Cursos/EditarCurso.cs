using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class EditarCurso
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }


        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Debe ingresar un titulo");
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
                var curso = await _context.Cursos.FindAsync(request.Id);
                if (curso == null)
                {
                    //throw new Exception("El curso no existe");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo; //si no le modifico el titulo que le deje el que ya tenia
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Actualizar Precio del curso
                var precio = _context.Precios.Where(x => x.CursoId == curso.Id).FirstOrDefault();
                if (precio != null)
                {
                    precio.PrecioActual = request.Precio ?? precio.PrecioActual;
                    precio.Promocion = request.Promocion ?? precio.Promocion;
                }
                else
                {
                    precio = new Precio
                    {
                        Id = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.Id
                    };
                    await _context.Precios.AddAsync(precio);
                }


                //Eliminar instructor para despues modificar
                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        //Eliminar instructores actuales del curso en la Tabla cursoInstructores
                        var instructoresBD = _context.CursoInstructores.Where(x => x.CursoId == request.Id).ToList();
                        foreach (var instructor in instructoresBD)
                        {
                            _context.CursoInstructores.Remove(instructor);
                        }
                        //fin eliminar

                        //Aqui agrego los nuevos instructores que ingresa el cliente
                        foreach (var id in request.ListaInstructor)
                        {
                            var nuevoinstructor = new CursoInstructor
                            {
                                CursoId = request.Id,
                                InstructorId = id
                            };
                            _context.CursoInstructores.Add(nuevoinstructor);
                        }
                    }
                }


                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}

