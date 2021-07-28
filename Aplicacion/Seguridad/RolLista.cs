using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>>
        {

        }


        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {

            private readonly CursosContext _context;
            public Manejador(CursosContext context)
            {
                _context = context;
            }

            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var roles = await _context.Roles.ToListAsync();
                if (roles == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "No existe ningun rol cargado en el sistema" });
                }

                return roles;

            }
        }
    }
}
