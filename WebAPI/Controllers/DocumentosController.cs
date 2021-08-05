using Aplicacion.Documentos;
using MediatR;
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
    public class DocumentosController : MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> GuardarArchivo(SubirArchivo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoGenerico>> ObtenerArchivo(Guid id)
        {
            return await Mediator.Send(new ObtenerArchivo.Ejecuta { Id = id});
        }
    }
}
