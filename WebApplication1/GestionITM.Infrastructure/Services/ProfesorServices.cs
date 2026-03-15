using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Exceptions;


namespace GestionITM.Infrastructure.Services
{
	public class ProfesorServices : IProfesorService  //la interfaz IEstudianteService define los métodos que esta clase debe implementar, este se encarga de buscar todo lo neceesario para preparar todo los alimentos
	{
		private readonly InterfaceProfeRepositorio _repository;
		private readonly IMapper _mapper;  //El IMapper es el asistente de la interfaz para poder pasar los datos crudos hacia los platos o solicitudes necesarias

		public ProfesorServices(InterfaceProfeRepositorio repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ProfesorDto>> ObtenerTodosLosProfesoresAsync()
		{
			var profesores = await _repository.ObtenerTodoAsync();
			return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
		}
		public async Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto)
		{

			//Reglas de negocio para validadr el Profesor Nivel 5
			//No permitimos correo que no sea del dominio itm.edu.co
			if (!profesorCreateDto.Correo.EndsWith("@correo.itm.edu.co"))
			{
				return false;
			}
		
			var existe = await _repository.ExistePorDocumentoAsync(profesorCreateDto.Documento);

			if (existe)
				throw new ConflictException($"Ya existe un Profesor con el documento {profesorCreateDto.Documento}"); // 409

			
			var profesor = _mapper.Map<Profesor>(profesorCreateDto);
			profesor.FechaContratacion = DateTime.UtcNow; //Asignamos la fecha de inscripción al momento de registrar

			await _repository.AgregarAsync(profesor);

			return true; // Profesor registrado exitosamente
		}

		public async Task<ProfesorDto?> ObtenerPorIdAsync(int ProfesorId)
		{
			var profesor = await _repository.ObtenerPorIdAsync(ProfesorId);

			if (profesor == null)
				throw new NotFoundException($"No se encontró el Profesor con ID {ProfesorId}"); // 404

			return _mapper.Map<ProfesorDto>(profesor); // se convierte la Entidad en Dto
		}


		public async Task<IEnumerable<ProfesorDto>> ObtenerProfesoresPorEspecialidadAsync(string Especialidad)
		{
			var profesores = await _repository.ObtenerProfesoresPorEspecialidadAsync(Especialidad);
			return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
		}


		public async Task<ProfesorDto?> ObtenerPorDocumentoAsync(string Documento)
		{
			var profesor = await _repository.ObtenerPorDocumentoAsync(Documento);

			if (profesor == null)
				return null;

			return _mapper.Map<ProfesorDto>(profesor); // se convierte la Entidad en Dto
		}

		public async Task<bool> ActualizarProfesorAsync(ProfesorUpdateDto profesorUpdateDto)
		{
			var profesor = await _repository.ObtenerPorIdAsync(profesorUpdateDto.ProfesorId);

			if (profesor == null)
				return false;

			//si encuentra al profesor entonces
			_mapper.Map(profesorUpdateDto, profesor);

			await _repository.ActualizarAsync(profesor);

			return true;
		}
		public async Task<bool> DeleteProfesorAsync(int ProfesorId)
		{
			var profesor = await _repository.ObtenerPorIdAsync(ProfesorId);

			if (profesor == null)
				return false;

			await _repository.EliminarAsync(ProfesorId);
			return true;
		}

	}
}
