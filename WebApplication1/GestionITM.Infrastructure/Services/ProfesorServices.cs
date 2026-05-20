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
using GestionITM.Domain.Modelos; //para el pagedresults
using Microsoft.EntityFrameworkCore; //Crucial para TolistAsync y CountAsync



namespace GestionITM.Infrastructure.Services
{
	public class ProfesorServices : IProfesorService  //la interfaz IEstudianteService define los métodos que esta clase debe implementar, este se encarga de buscar todo lo neceesario para preparar todo los alimentos
	{
		private readonly InterfaceProfeRepositorio _repository;
		private readonly InterfaceProfeRepositorio _profesorRepository;
		private readonly IMapper _mapper;  //El IMapper es el asistente de la interfaz para poder pasar los datos crudos hacia los platos o solicitudes necesarias

		public ProfesorServices(InterfaceProfeRepositorio repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}
		//1. Obtener Todo(Get All (Sin Paginacion) - Para Uso Administrativo interno)

		public async Task<IEnumerable<ProfesorDto>> ObtenerTodosLosProfesoresAsync()
		{
			var profesores = await _repository.ObtenerTodoAsync();
			return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
		}


		//2. Obtener el Paginado y Filtrado(El Standard de alto rendimiento para la mayoria de los casos de Uso)
		//Nivel 5: IQueryable + Skip/Take en SQL Server
		public async Task<PagedResults<ProfesorDto>> ObtenerProfesoresPaginadosAsync(ProfesorFilterDto filter)
		{
			//Fase A: Preparar la consulta (IQueryable) con los filtros aplicados
			// Todavia No se ha ejecutado Nada en SQL, solo se ha construido la Expresion de consulta
			var consulta = _repository.ConsultarTodo();

			//Fase B: Aplicar filtros dinamicos
			if (!string.IsNullOrEmpty(filter.BusquedaPorNombre))
			{
				consulta = consulta.Where(p => p.Name.Contains(filter.BusquedaPorNombre));
			}
			if (!string.IsNullOrEmpty(filter.Especialidad))
			{
				consulta = consulta.Where(p => p.Especialidad == filter.Especialidad);
			}

			// Fase C: Conteo total de Registros para la paginacion
			//Necesario para el Frontend sepa cuantas paginas hay en total
			var totalRegistros = await consulta.CountAsync();

			//Fase D: Aplicar paginacion
			//solo traemos los registros que caben en la pantalla del usuario.

			var items = await consulta
				.Skip((filter.Pagina - 1) * filter.RegistrosPorPagina)
				.Take(filter.RegistrosPorPagina)
				.ToListAsync();

			//Fase E: Empaquedado final en un PagedResult
			return new PagedResults<ProfesorDto>
			{
				Items = _mapper.Map<List<ProfesorDto>>(items),
				PaginaActual = filter.Pagina,
				TotalRegistros = totalRegistros,
				RegistrosPorPagina = filter.RegistrosPorPagina,
				TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filter.RegistrosPorPagina)
			};

		}

		// 3. Registrar Profesor(reglas de negocio)
		public async Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto)
		{
			//PRUEBA - Lanza excepción si el nombre es "Error"
			if (profesorCreateDto.NombreCompleto.Trim().ToLower() == "error")
			{
				throw new Exception("Error de prueba");
			}

			//Reglas de negocio para validadr el Profesor Nivel 5
			//No permitimos correo que no sea del dominio itm.edu.co
			if (!profesorCreateDto.Correo.EndsWith("@correo.itm.edu.co"))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(profesorCreateDto.Especialidad))
			{
				throw new ArgumentException("La especialidad del profesor no puede estar vacía.");
			}

			if (profesorCreateDto.Especialidad.Trim().ToLower() == "arquitectura")
			{
				Console.WriteLine("Perfil Senior Detectado");
			}

			var existe = await _repository.ExistePorDocumentoAsync(profesorCreateDto.Documento);

			if (existe)
				throw new ConflictException($"Ya existe un Profesor con el documento {profesorCreateDto.Documento}"); // 409

			
			var profesor = _mapper.Map<Profesor>(profesorCreateDto);
			profesor.FechaContratacion = DateTime.UtcNow; //Asignamos la fecha de inscripción al momento de registrar

			await _repository.AgregarAsync(profesor);

			return true; // Profesor registrado exitosamente
		}

		//4. Obtener po ID

		public async Task<ProfesorDto?> ObtenerPorIdAsync(int ProfesorId)
		{
			var profesor = await _repository.ObtenerPorIdAsync(ProfesorId);

			if (profesor == null)
				throw new NotFoundException($"No se encontró el Profesor con ID {ProfesorId}"); // 404

			return _mapper.Map<ProfesorDto>(profesor); // se convierte la Entidad en Dto
		}

		//5. Obtener por especialidad
		public async Task<IEnumerable<ProfesorDto>> ObtenerProfesoresPorEspecialidadAsync(string Especialidad)
		{
			var profesores = await _repository.ObtenerProfesoresPorEspecialidadAsync(Especialidad);
			return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
		}

		//6. Obtener por documento
		public async Task<ProfesorDto?> ObtenerPorDocumentoAsync(string Documento)
		{
			var profesor = await _repository.ObtenerPorDocumentoAsync(Documento);

			if (profesor == null)
				return null;

			return _mapper.Map<ProfesorDto>(profesor); // se convierte la Entidad en Dto
		}

		//8. Actualizar Profesor 
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

		//8. Eliminar Profesor
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
