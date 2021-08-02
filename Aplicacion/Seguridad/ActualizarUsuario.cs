using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class ActualizarUsuario
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }


        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public Manejador(CursosContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _passwordHasher = passwordHasher;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);

                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = $"No existe un usuario con el nombre {request.UserName}" });
                }

                //valido de que el email no lo tenga otro usuario q sea unico
                var email = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();
                if (email)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = $"Ya existe un usuario con el email {request.Email}" });
                }

                usuario.NombreCompleto = request.NombreCompleto;
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.Password); //De esta manera encrypto el password
                usuario.Email = request.Email;

                var resultado = await _userManager.UpdateAsync(usuario);

                //obtener lista de roles
                var roles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(roles);

                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles)
                    };
                }

                throw new Exception($"No se pudo actualizar el usuario {request.UserName}");
            }
        }
    }
}
