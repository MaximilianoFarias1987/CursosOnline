using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly IMediator _mediador;

        public CursosController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> GetCursos()
        {
            return await _mediador.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCursoId(int id)
        {
            return await _mediador.Send(new ConsultaId.CursoId { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> InsertarCurso(InsertarCurso.Ejecuta data)
        {
            return await _mediador.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> ActualizarCurso(int id, EditarCurso.Ejecuta data)
        {
            data.Id = id;
            return await _mediador.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> EliminarCurso(int id)
        {
            return await _mediador.Send(new EliminarCurso.Ejecuta { Id = id });
        }
    }
}
