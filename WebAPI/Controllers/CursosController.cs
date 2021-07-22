using Aplicacion.Cursos;
using Aplicacion.Cursos.DTOcursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {
        //private readonly IMediator _mediador;

        //public CursosController(IMediator mediador)
        //{
        //    _mediador = mediador;
        //}

        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> GetCursos()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCursoId(Guid id)
        {
            return await Mediator.Send(new ConsultaId.CursoId { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> InsertarCurso(InsertarCurso.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> ActualizarCurso(Guid id, EditarCurso.Ejecuta data)
        {
            data.Id = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> EliminarCurso(Guid id)
        {
            return await Mediator.Send(new EliminarCurso.Ejecuta { Id = id });
        }
    }
}
