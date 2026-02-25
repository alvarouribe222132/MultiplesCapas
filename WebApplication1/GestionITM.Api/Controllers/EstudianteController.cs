using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;


namespace GestionITM.Api.Controllers
{
	[Route ("api/controller")]
	[ApiController]

	public class EstudianteController  :ControllerBase
	{
		private readonly InterfaceEstudRepositorio _repository;
			public EstudianteController(InterfaceEstudRepositorio repository)
			{
				_repository = repository;
		     }
		//Get api/estudiante
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiante()
		{
			var estudiantes = await _repository.ObtenerTodoAsync();
			return Ok(estudiantes);
		}

		//Get api/estudiante/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Estudiante>> GetEstudientes(int id)
		{
			var estudiante = await _repository.ObtenerPorIdAsync(id);
			if (estudiante != null)
			{
				return NotFound(new { message = $"Estudiante con el {id} no fue encontrado" });
			}
			return Ok(estudiante);
		}

		//Post api/estudiante
		[HttpPost]

		public async Task<ActionResul> PostEstudiante(Estudiante estudiante)
		{
			await _repository.AgregarAsync
		}
	}
}