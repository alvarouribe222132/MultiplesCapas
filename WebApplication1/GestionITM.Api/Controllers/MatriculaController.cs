using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionITM.Api.Controllers  //este controler solo recibe peticiones Http,
									  //y se encarga de llamar al servicio para que este ejecute la logica de negocio,
									  //y luego devuelve una respuesta Http al cliente.
									  //Es el encargado de manejar la comunicacion entre el cliente y el servicio.
{
	[Route("api/[controller]")] // esta es la ruta base para acceder a este controlador, por ejemplo api/Matricula
	[ApiController]
	public class MatriculaController : ControllerBase
	{//1. ahora solo dependemos del la interfaz del servicio no del repositorio 
		private  readonly IMatriculaService _service;

		//2. inyectamos el servicio en lugar del repositorio. El contructor ahora es mucha mas limpio
		public MatriculaController(IMatriculaService service)
		{
			_service = service;
		}
		// GET: Api/Matricula
		[HttpGet()]

		public async Task<ActionResult<IEnumerable<MatriculaDto>>> GetMatriculas()
		{
			var matriculas = await _service.ObtenerTodasLasMatriculasAsync();
			return Ok(matriculas); //devuelve el codigo 200 y la lista de cursos
		}


		//GetApi/Matricula/5
		[HttpGet("{IdMatricula}")]
		public async Task<ActionResult<MatriculaDto>> GetMatricula(int IdMatricula)
		{
			var matricula = await _service.ObtenerPorIdAsync(IdMatricula);
			if (matricula == null)
			{
				return NotFound(new { message = $"La matrícula con el ID {IdMatricula} no fue encontrada" });
			}
			return Ok(matricula);// devuelve la matrícula encontrada
		}

		//Post api/Matricula
		[HttpPost]
		public async Task<ActionResult> PostMatricula(MatriculaCreateDto matriculaCreateDto)
		{
			var IdMatricula = await _service.RegistrarMatriculaAsync(matriculaCreateDto);
			//Devuelve el codigo 201 y la ruta para acceder a la ubicacion con el nuevo curso creado

			return CreatedAtAction(nameof(GetMatricula), new { IdMatricula  = IdMatricula}, matriculaCreateDto);
		}


		//Put api/Matricula
		[HttpPut("{id}")]

		public async Task<ActionResult> PutMatricula(int id, MatriculaUpdateDto matriculaUpdateDto)
		{
			if (id != matriculaUpdateDto.MatriculaId)
			{
				return BadRequest(new { message = $"El ID de la matrícula {id} en la URL no coincide con el ID en el cuerpo de la solicitud" });
			}
			var actualizado = await _service.ActualizarMatriculaAsync(matriculaUpdateDto);

			if (!actualizado)
			{
				return NotFound();
			}
			return NoContent(); //devuelve el codigo 204 que significa que la solicitud se realizo correctamente pero no hay contenido para devolver
		}

		//Delete api/Matricula
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteMatricula(int id)
		{
			var eliminado = await _service.DeleteMatriculaAsync(id);
			if (!eliminado)
			{
				return NotFound();
			}
			return NoContent(); //devuelve el codigo 204 que significa que la solicitud se realizo correctamente pero no hay contenido para devolver
		}

		[HttpGet("curso/{curso}")]
		public async Task<ActionResult<IEnumerable<MatriculaDto>>>GetMatriculaPorCurso(string curso)
		{
			var matriculas =await _service.ObtenerMatriculaPorCursoAsync(curso);
			return Ok(matriculas);
		}

		//Filtrado/Paginacion
		[HttpGet("paginado")]
		public async Task<ActionResult<PagedResults<MatriculaDto>>>GetPaginado([FromQuery] MatriculaFilterDto filter)
		{
			var resultado =	await _service.ObtenerMatriculasPaginadasAsync(filter);
			return Ok(resultado);
		}
	}

}
