using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;


namespace GestionITM.Infrastructure.Services
{
	public class EstudianteServices : IEstudianteService  //la interfaz IEstudianteService define los métodos que esta clase debe implementar, este se encarga de buscar todo lo neceesario para preparar todo los alimentos
	{
		private readonly InterfaceEstudRepositorio _repository;
		private readonly IMapper _mapper;  //El IMapper es el asistente de la interfaz para poder pasar los datos crudos hacia los platos o solicitudes necesarias

		public EstudianteServices(InterfaceEstudRepositorio repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EstudianteDto>> ObtenerTodosLosEstudiantesAsync()
		{
			var estudiantes = await _repository.ObtenerTodoAsync();
			return _mapper.Map<IEnumerable<EstudianteDto>>(estudiantes);
		}
		public async Task<bool> RegistrarEstudianteAsync(EstudianteCreateDto estudianteCreateDto)
		{

			//Reglas de negocio para validadr el estudiante Nivel 5
			//No permitimos correo que no sea del dominio itm.edu.co

			if (!estudianteCreateDto.Correo.EndsWith("@correo.itm.edu.co"))
			{
				return false; //No se puede registrar el estudiante
			}
			var estudiante = _mapper.Map<Estudiante>(estudianteCreateDto);
			estudiante.FechaInscripcion = DateTime.UtcNow; //Asignamos la fecha de inscripción al momento de registrar

			await _repository.AgregarAsync(estudiante);

			return true; // Estudiante registrado exitosamente
		}

		public async Task<EstudianteDto?> ObtenerPorIdAsync(int EstudianteId)
		{
			var estudiante = await _repository.ObtenerPorIdAsync(EstudianteId);
			if (estudiante == null)
			{
				return null; // No se encontró el estudiante
			}
			return _mapper.Map<EstudianteDto>(estudiante);
		} 
	}
}
