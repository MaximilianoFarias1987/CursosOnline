using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : MiControllerBase
    {
        [HttpPost("crearRol")]
        public async Task<ActionResult<Unit>> CrearRol(RolNuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpDelete("eliminarRol")]
        public async Task<ActionResult<Unit>> EliminarRol(RolEliminar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpGet("listadoRoles")]
        public async Task<ActionResult<List<IdentityRole>>> ListaRoles()
        {
            return await Mediator.Send(new RolLista.Ejecuta());
        }

        [HttpPost("agregarRolUsuario")]
        public async Task<ActionResult<Unit>> AgregarRolUsuario(AgregarRolUsuario.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPost("eliminarRolUsuario")]
        public async Task<ActionResult<Unit>> EliminarRolUsuario(EliminarRolUsuario.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<List<string>>> ObtenerRolUsuario(string userName)
        {
            return await Mediator.Send(new ObtenerRolesPorUsuario.Ejecuta { UserName = userName });
        }

    }
}
