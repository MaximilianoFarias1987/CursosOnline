using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
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
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData> { }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;
            private readonly CursosContext _cursosContext;
            public Manejador(CursosContext cursosContext, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                _cursosContext = cursosContext;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "No hay ningún usuario en sesión" });
                }

                var roles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(roles);

                var imagenPerfil = await _cursosContext.Documentos.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();
                if (imagenPerfil != null)
                {
                    var imagen = new ImagenGeneral
                    {
                        Data = System.Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };



                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        UserName = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                        Email = usuario.Email,
                        ImagenPerfil = imagen
                    };
                }
                else
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        UserName = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                        Email = usuario.Email
                    };
                }




            }
        }
    }
}
