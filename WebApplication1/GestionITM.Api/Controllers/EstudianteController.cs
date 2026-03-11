using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;


//aqui estamos contrullendo un controlador en capas donde el controlador pertenece a la api, pero depende de las capas de dominio e infraestructura donde estas las entidades y interfaces para funcionar,
//esto es una buena practica de diseño de software porque nos permite separar las responsabilidades y hacer que el codigo sea mas mantenible y escalable

namespace GestionITM.Api.Controllers
{
	[Authorize]//ESTE es el candado del 2 filtro de seguridad
	[Route ("api/[controller]")] // esta es la ruta base para acceder a este controlador, por ejemplo api/estudiante
	[ApiController]

	public class EstudianteController  : ControllerBase
	{
		//1. ahora solo dependemos del la interfaz del servicio no del repositorio 

		private readonly IEstudianteService _service;

		//2. inyectamos el servicio en lugar del repositorio. El contructor ahora es mucha mas limpio
		public EstudianteController(IEstudianteService service)
		{ 
			_service = service;
		}



		//Get api/estudiante
		[HttpGet]
		public async Task<ActionResult<IEnumerable<EstudianteDto>>> GetEstudiante()
		{
			var estudiantesDto = await _service.ObtenerTodosLosEstudiantesAsync();
			return Ok(estudiantesDto); //devuelve el codigo 200 y la lista de estudiantes
		}

		//Get api/estudiante/5
		[HttpGet("{EstudianteId:int}")]
		public async Task<ActionResult<EstudianteDto>> GetEstudiente(int EstudianteId)
		{
			var estudianteDto = await _service.ObtenerPorIdAsync(EstudianteId);
			if (estudianteDto == null)
			{
				return NotFound(new { message = $"Estudiante con el {EstudianteId} no fue encontrado" });
			}
			return Ok(estudianteDto);// devuelve el estudiante encontrado
		}

		//Post api/estudiante
		[HttpPost]

		public async Task<ActionResult> PostEstudiante([FromBody]EstudianteCreateDto estudianteCreateDto)
		{
			// 3. El servicio valida la logica (como el correo @itm) y guarda  
			var resultado = await _service.RegistrarEstudianteAsync(estudianteCreateDto);

			if (!resultado)
			{
				return BadRequest("No se pudo registrar. Verifique que el correo sea institucional (@correo.itm.edu.co)");
			}
			//En un flujo Novel 5 real, el servicio podria devolver el estudiante creado con su id generado, y aqui podriamos devolver un CreatedAtAction con la ruta para obtener ese estudiante por id, pero por simplicidad solo devolvemos un Ok
			return Ok(new {message = "Estudiante registrado con exito en el sistema" });
		}
	}
}

//ActionResult es un contenedor que nos permite devolver tanto el objeto (el contenido, el nombre del estudiante) como el codigo de estado HTTP (200, 201, 404, etc.)
//en una sola respuesta. Esto es útil para comunicar claramente el resultado de la operación al cliente que consume la API.