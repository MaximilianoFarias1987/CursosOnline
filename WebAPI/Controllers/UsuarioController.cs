using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] //Esta anotacion me permite que sea publico este metodo
    public class UsuarioController : MiControllerBase
    {
        //http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> RegistrarUsuario(RegistrarUsuario.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpGet]
        public async Task<ActionResult<UsuarioData>> ObtenerUsuarioActual()
        {
            return await Mediator.Send(new UsuarioActual.Ejecuta());
        }


        [HttpPut("actualizar")]
        public async Task<ActionResult<UsuarioData>> ActualizarUsuario(ActualizarUsuario.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
    }
}
