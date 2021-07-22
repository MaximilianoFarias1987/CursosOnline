using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }


        //VALIDACION
        public class Validacion : AbstractValidator<Ejecuta>
        {
            public Validacion()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("Debe ingresar un Email"); ;
                RuleFor(x => x.Password).NotEmpty().WithMessage("Debe ingresar un Password"); ;
            }
        }


        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;

            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = "esta sera la data del token",
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}

