using Aplicacion.Instructores;
using Aplicacion.Instructores.DTOinstructores;
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
    public class InstructoresController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<InstructorDTO>>> GetInstructor()
        {
            return await Mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDTO>> GetInstructorId(Guid id)
        {
            return await Mediator.Send(new ConsultaInstructorId.Ejecuta { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> InsertarInstructor(InsertarInstructor.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
    }
}
