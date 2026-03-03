using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;

//aqui estamos contrullendo un controlador en capas donde el controlador pertenece a la api, pero depende de las capas de dominio e infraestructura donde estas las entidades y interfaces para funcionar,
//esto es una buena practica de diseño de software porque nos permite separar las responsabilidades y hacer que el codigo sea mas mantenible y escalable

namespace GestionITM.Api.Controllers
{
	[Route ("api/controller")] // esta es la ruta base para acceder a este controlador, por ejemplo api/estudiante
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
			return Ok(estudiantes); //devuelve el codigo 200 y la lista de estudiantes
		}

		//Get api/estudiante/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Estudiante>> GetEstudiente(int EstudianteId)
		{
			var estudiante = await _repository.ObtenerPorIdAsync(EstudianteId);
			if (estudiante == null)
			{
				return NotFound(new { message = $"Estudiante con el {EstudianteId} no fue encontrado" });
			}
			return Ok(estudiante);// devuelve el estudiante encontrado
		}

		//Post api/estudiante
		[HttpPost]

		public async Task<ActionResult> PostEstudiante(Estudiante estudiante)
		{
			await _repository.AgregarAsync(estudiante);
			//Devuelve el codigo 201 y la ruta para acceder a la ubicacion con el nuevo estudiante creado

			return CreatedAtAction(nameof(GetEstudiente), new { id = estudiante.EstudianteId }, estudiante);

		}
	}
}

//ActionResult es un contenedor que nos permite devolver tanto el objeto (el contenido, el nombre del estudiante) como el codigo de estado HTTP (200, 201, 404, etc.)
//en una sola respuesta. Esto es útil para comunicar claramente el resultado de la operación al cliente que consume la API.