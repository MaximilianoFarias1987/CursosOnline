using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class RolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rol = await _roleManager.FindByNameAsync(request.Nombre);
                if (rol == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "No existe el rol a eliminar" });
                }

                var resultado = await _roleManager.DeleteAsync(rol);
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el rol");
            }
        }
    }
}
