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
            private readonly IJwtGenerador _jwtGenerador;
            private readonly CursosContext _cursosContext;

            public Manejador(CursosContext cursosContext,UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                _cursosContext = cursosContext;
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                var roles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(roles);
                var imagenPerfil = await _cursosContext.Documentos.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                if (resultado.Succeeded)
                {
                    if (imagenPerfil != null)
                    {
                        var imagen = new ImagenGeneral
                        {
                            Data = Convert.ToBase64String(imagenPerfil.Contenido),
                            Extension = imagenPerfil.Extension,
                            Nombre = imagenPerfil.Nombre
                        };

                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                            UserName = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = imagen
                        };

                    }
                    else
                    {
                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                            UserName = usuario.UserName,
                            Email = usuario.Email
                        };
                    }
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}

