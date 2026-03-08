using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure.Repositorios;





namespace GestionITM.Infrastructure.Services
{
	public class EstudianteServices : IEstudianteService
	{
		private readonly IEstudianteService _repository;
		private readonly IMapper _mapper;

		public EstudianteServices(IEstudianteService repository, IMapper mapper)
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

			if (!estudianteCreateDto.Correo.EndsWith("@itm.edu.co"))
			{
				return false; //No se puede registrar el estudiante
			}
			var estudiante = _mapper.Map<Estudiante>(estudianteCreateDto);
			estudiante.FechaInscripcion = DateTime.UtcNow; //Asignamos la fecha de inscripción al momento de registrar

			await _repository.AgregarAsync(estudiante);

			return true; // Estudiante registrado exitosamente
		}
	}
}
