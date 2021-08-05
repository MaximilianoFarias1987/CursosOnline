using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Documentos
{
    public class ObtenerArchivo
    {
        public class Ejecuta : IRequest<ArchivoGenerico>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, ArchivoGenerico>
        {
            private readonly CursosContext _context;

            public Manejador(CursosContext context)
            {
                _context = context;
            }
            public async Task<ArchivoGenerico> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var archivo = await _context.Documentos.Where(x => x.ObjetoReferencia == request.Id).FirstAsync();
                if (archivo == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new { mensaje = "No se encontro el archivo" });
                }

                var archivoGenerico = new ArchivoGenerico
                {
                    Data = Convert.ToBase64String(archivo.Contenido),
                    Nombre = archivo.Nombre,
                    Extencion = archivo.Extension
                };

                return archivoGenerico;
            }
        }
    }
}
