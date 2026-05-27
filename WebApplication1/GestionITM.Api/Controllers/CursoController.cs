using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Modelos;
using GestionITM.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Api.Controllers
{
	[Route("api/[controller]")] // esta es la ruta base para acceder a este controlador, por ejemplo api/curso
	[ApiController]
	public class CursoController : ControllerBase
	{
		private  readonly ICursoService _service;
		private readonly ApplicationDbContext _context;
		public CursoController(ICursoService service, ApplicationDbContext context)
		{
			_service = service;
			_context = context;
		}
		// GET: Api/Curso
		[HttpGet()]

		public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
		{
			var cursos = await _service.ObtenerTodosLosCursosAsync();
			return Ok(cursos); //devuelve el codigo 200 y la lista de cursos
		}

		//GetApi/Curso/5
		[HttpGet("{IdCurso}")]
		public async Task<ActionResult<CursoDto>> GetCurso(int IdCurso)
		{
			var curso = await _service.ObtenerPorIdAsync(IdCurso);
			if (curso == null)
			{
				return NotFound(new { message = $"El Curso con el ID {IdCurso} no fue encontrado" });
			}
			return Ok(curso);// devuelve el CURSO encontrado
		}

		//Post api/Curso
		[HttpPost]
		public async Task<ActionResult> PostCurso(CursoCreateDto cursoCreateDto)
		{
			var id = await _service.RegistrarCursoAsync(cursoCreateDto);
			//Devuelve el codigo 201 y la ruta para acceder a la ubicacion con el nuevo curso creado

			return CreatedAtAction(nameof(GetCurso), new { idCurso = id }, cursoCreateDto);
		}

		[HttpGet("paginado")]
		public async Task<ActionResult<PagedResults<CursoDto>>> GetPaginado(
			[FromQuery] CursoFilterDto filter)
		{
			var resultado = await _service.ObtenerCursosPaginadosAsync(filter);

			return Ok(resultado);
		}

		[HttpPut("{id}/actualizar-cupos")]
		public async Task<IActionResult> UpdateCupos(int id, [FromBody] int nuevosCupos)
		{
			// 1. Buscar el curso en la base de datos
			var curso = await _context.Cursos.FindAsync(id);

			if (curso == null)
				return NotFound("El curso no existe.");

			// 2. Actualizar el valor
			curso.CuposDisponibles = nuevosCupos;

			// 3. Guardar cambios
			await _context.SaveChangesAsync();

			return Ok(new { message = "Cupos actualizados correctamente", cursoId = id, nuevosCupos });
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> PutCurso(int id, CursoUpdateDto cursoUpdateDto)
		{
			if (id != cursoUpdateDto.IdCurso)
				return BadRequest(new { message = "El ID no coincide" });

			var actualizado = await _service.ActualizarCursoAsync(cursoUpdateDto);
			if (!actualizado)
				return NotFound(new { message = $"El curso con ID {id} no existe" });

			return Ok(new { message = "Curso actualizado correctamente" });
		}

	}
}
