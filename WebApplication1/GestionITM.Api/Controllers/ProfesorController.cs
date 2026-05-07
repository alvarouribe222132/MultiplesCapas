using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionITM.Api.Controllers
{
	//[Authorize] 07/05/2026 se desactiva temporalmente para pruebas
	[Route("api/[controller]")]
	[ApiController]
	public class ProfesorController : ControllerBase
	{
		private readonly IProfesorService _service;

		public ProfesorController(IProfesorService service)
		{
			_service = service;
		}

		// GET: api/profesor
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProfesorDto>>> GetProfesor()
		{
			var profesoresDto = await _service.ObtenerTodosLosProfesoresAsync();
			return Ok(profesoresDto);
		}

		// GET: api/profesor/paginado?especialidad=Sistemas&pagina=1
		[HttpGet("paginado")]
		public async Task<ActionResult> GetPaginado([FromQuery] ProfesorFilterDto filtro)
		{
			var resultado = await _service.ObtenerProfesoresPaginadosAsync(filtro);
			return Ok(resultado);
		}

		// GET: api/profesor/5
		[HttpGet("{ProfesorId:int}")]
		public async Task<ActionResult<ProfesorDto>> GetProfesorporId(int ProfesorId)
		{
			var profesorDto = await _service.ObtenerPorIdAsync(ProfesorId);
			if (profesorDto == null)
				return NotFound(new { message = $"El Profesor con el id {ProfesorId} no fue encontrado" });

			return Ok(profesorDto);
		}

		// GET: api/profesor/documento/1234567
		[HttpGet("documento/{Documento}")]
		public async Task<ActionResult<ProfesorDto>> GetProfesorPorDocumento(string Documento)
		{
			var profesorDto = await _service.ObtenerPorDocumentoAsync(Documento);
			if (profesorDto == null)
				return NotFound(new { message = $"El Profesor con documento {Documento} no fue encontrado" });

			return Ok(profesorDto);
		}

		// POST: api/profesor
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult> PostProfesor([FromBody] ProfesorCreateDto profesorCreateDto)
		{
			var resultado = await _service.RegistrarProfesorAsync(profesorCreateDto);
			if (!resultado)
				return BadRequest("No se pudo registrar. Verifique los datos ingresados.");

			return Ok(new { message = "Profesor registrado con éxito en el sistema" });
		}

		// PUT: api/profesor/5
		[HttpPut("{ProfesorId:int}")]
		public async Task<ActionResult> PutProfesor(int ProfesorId, [FromBody] ProfesorUpdateDto profesorUpdateDto)
		{
			if (ProfesorId != profesorUpdateDto.ProfesorId)
				return BadRequest("El ID ingresado no coincide con el ID del profesor.");

			var actualiza = await _service.ActualizarProfesorAsync(profesorUpdateDto);
			if (!actualiza)
				return NotFound(new { message = $"El profesor con id {ProfesorId} no existe." });

			return Ok(new { message = "Profesor actualizado correctamente" });
		}

		// DELETE: api/profesor/5
		[HttpDelete("{ProfesorId:int}")]
		public async Task<ActionResult> DeleteProfesor(int ProfesorId)
		{
			var eliminado = await _service.DeleteProfesorAsync(ProfesorId);
			if (!eliminado)
				return NotFound(new { message = $"El profesor con id {ProfesorId} no existe." });

			return Ok(new { message = "Profesor eliminado correctamente" });
		}
	}
}