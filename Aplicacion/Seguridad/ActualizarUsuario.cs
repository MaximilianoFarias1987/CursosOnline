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
            public ImagenGeneral ImagenPerfil { get; set; }
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

                if (request.ImagenPerfil != null)
                {
                    //Valido si hay o no una imagen o documento
                    var resultadoImagen = await _context.Documentos.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();
                    if (resultadoImagen == null)
                    {
                        //Si es null agrego la nueva imagen
                        var imagen = new Documento
                        {
                            Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data),
                            Nombre = request.ImagenPerfil.Nombre,
                            Extension = request.ImagenPerfil.Extension,
                            ObjetoReferencia = new Guid(usuario.Id),
                            Id = Guid.NewGuid(),
                            FechaCreacion = DateTime.UtcNow
                        };

                        _context.Documentos.Add(imagen);
                    }
                    else
                    {
                        //Si no es null, la actualizo o dejo la que ya esta
                        resultadoImagen.Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data);
                        resultadoImagen.Nombre = request.ImagenPerfil.Nombre;
                        resultadoImagen.Extension = request.ImagenPerfil.Extension;
                    }
                }



                usuario.NombreCompleto = request.NombreCompleto;
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.Password); //De esta manera encrypto el password
                usuario.Email = request.Email;

                var resultado = await _userManager.UpdateAsync(usuario);

                //obtener lista de roles
                var roles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(roles);

                var imagenPerfil = await _context.Documentos.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstAsync();

                ImagenGeneral imagenGeneral = null;
                if (imagenPerfil != null)
                {
                    imagenGeneral = new ImagenGeneral
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Nombre = imagenPerfil.Nombre,
                        Extension = imagenPerfil.Extension
                    };


                }
                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                        ImagenPerfil = imagenGeneral
                    };
                }

                throw new Exception($"No se pudo actualizar el usuario {request.UserName}");
            }
        }
    }
}
