using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class EliminarRolUsuario
    {
        public class Ejecuta : IRequest
        {
            public string UserName { get; set; }
            public string RolNombre { get; set; }
        }

        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<Usuario> _userManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var rol = await _roleManager.FindByNameAsync(request.RolNombre);
                if (rol == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El rol " + request.RolNombre + " no existe" });
                }

                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El usuario " + request.UserName + " no existe" });
                }

                var resultado = await _userManager.RemoveFromRoleAsync(usuario, request.RolNombre);
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el rol " + request.RolNombre + " al usuario " + request.RolNombre);
            }
        }
    }
}
