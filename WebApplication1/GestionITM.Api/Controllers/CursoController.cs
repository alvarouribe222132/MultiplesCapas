using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;

namespace GestionITM.Api.Controllers
{
	[Route("api/[controller]")] // esta es la ruta base para acceder a este controlador, por ejemplo api/curso
	[ApiController]
	public class CursoController : ControllerBase
	{
		private  readonly ICursoRepository _repository;

		public CursoController(ICursoRepository repository)
		{
			_repository = repository;
		}
		// GET: Api/Curso
		[HttpGet()]

		public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
		{
			var cursos = await _repository.ObtenerTodoAsync();
			return Ok(cursos); //devuelve el codigo 200 y la lista de cursos
		}

		//GetApi/Curso/5
		[HttpGet("{IdCurso}")]
		public async Task<ActionResult<Curso>> GetCursos(int IdCurso)
		{
			var curso = await _repository.ObtenerPorIdAsync(IdCurso);
			if (curso == null)
			{
				return NotFound(new { message = $"El Curso con el ID {IdCurso} no fue encontrado" });
			}
			return Ok(curso);// devuelve el CURSO encontrado
		}

		//Post api/Curso
		[HttpPost]
		public async Task<ActionResult> PostCurso(Curso curso)
		{
			await _repository.AgregarAsync(curso);
			//Devuelve el codigo 201 y la ruta para acceder a la ubicacion con el nuevo curso creado

			return CreatedAtAction(nameof(GetCursos), new { id = curso.IdCurso }, curso);
		}

	}
}
