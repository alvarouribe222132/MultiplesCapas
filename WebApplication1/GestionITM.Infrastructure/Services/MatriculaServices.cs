using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Exceptions;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Modelos; //para el pagedresults
using Microsoft.EntityFrameworkCore; //Crucial para TolistAsync y CountAsync
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// que hace este services. (logica de negocio) como: reglas,validaciones,flujos,permisos y decisiones

namespace GestionITM.Infrastructure.Services
{
	public class MatriculaServices : IMatriculaService  //la interfaz IMatriculaService define los métodos que esta clase debe implementar,
														//este se encarga de buscar todo lo necesario para preparar todo los alimentos
	{
		private readonly InterfaceEstudRepositorio _estudianteRepository;   //Para preguntar si existe el Estudiante antes de
		private readonly InterfaceCursoRepositorio _cursoRepository;      //Para preguntar si existe el curso antes de
		private readonly InterfaceMatricRepositorio _repository;         
		private readonly IMapper _mapper;  //El IMapper es el asistente de la interfaz para poder pasar los datos crudos hacia los platos o solicitudes necesarias


		//constructor  para decirle al sistema que antes de crear una matricula, tambien necesito acceso a estudiantes y cursos.
		public MatriculaServices(InterfaceMatricRepositorio repository,	InterfaceEstudRepositorio estudianteRepository,InterfaceCursoRepositorio cursoRepository, IMapper mapper)
		{
			_repository = repository;
			_estudianteRepository = estudianteRepository;
			_cursoRepository = cursoRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<MatriculaDto>> ObtenerTodasLasMatriculasAsync()
		{
			var matriculas = await _repository.ObtenerTodoAsync();
			return _mapper.Map<IEnumerable<MatriculaDto>>(matriculas);

		}

		public async Task<MatriculaDto?> ObtenerPorIdAsync(int matriculaId)
		{
			var matricula = await _repository.ObtenerPorIdAsync(matriculaId);

			if (matricula == null)
			{
				throw new NotFoundException($"No se encontró la matrícula con ID {matriculaId}"); //404
			}

			return _mapper.Map<MatriculaDto>(matricula); // se convierte la Entidad en Dto
		}


		public async Task<int> RegistrarMatriculaAsync(MatriculaCreateDto matriculaCreateDto)
		{    //primero validar si el estudiante existe
			var estudiante = await _estudianteRepository.ObtenerPorIdAsync(matriculaCreateDto.EstudianteId);
			if (estudiante == null)
			{
				throw new NotFoundException($"No Existe el estudiante con ID {matriculaCreateDto.EstudianteId}"); // 404
			}
			//segundo validar si el curso existe
			var curso = await _cursoRepository.ObtenerPorIdAsync(matriculaCreateDto.CursoId);
			if (curso == null)
			{
				throw new NotFoundException($"No Existe el curso con ID {matriculaCreateDto.CursoId}"); // 404
			}

			// Validar si ya existe matrícula para ese estudiante y curso (Duplicado)
			var existe = await _repository.ExisteMatriculaAsync(matriculaCreateDto.EstudianteId, matriculaCreateDto.CursoId);

			if (existe)
			{
				throw new ConflictException($"Ya existe una matrícula para el estudiante {matriculaCreateDto.EstudianteId} en el curso {matriculaCreateDto.CursoId}"); // 409
			}

			//crear la matricula luego de las validaciones anteriores
			var matricula = _mapper.Map<Matricula>(matriculaCreateDto); // se convierte el Dto en Entidad

			matricula.FechaMatricula = DateTime.UtcNow; // se asigna la fecha actual
			matricula.Estado = "Activa"; // se asigna el estado inicial
			matricula.Periodo = matriculaCreateDto.Periodo;

			await _repository.CrearAsync(matricula); // se guarda en la base de datos
			return matricula.Id;
			//
		}

		public async Task<bool> ActualizarMatriculaAsync(MatriculaUpdateDto matriculaUpdateDto)
		{

			var matricula = await _repository.ObtenerPorIdAsync(matriculaUpdateDto.MatriculaId);

			if (matricula == null)
			{
				throw new NotFoundException($"La matrícula con id {matriculaUpdateDto.MatriculaId} no existe."); // 404
			}

			// Actualizar datos permitidos
			_mapper.Map(matriculaUpdateDto, matricula);

			await _repository.ActualizarAsync(matricula);

			return true;
		}
		public async Task<bool> DeleteMatriculaAsync(int matriculaId)
		{
			var matricula = await _repository.ObtenerPorIdAsync(matriculaId);

			if (matricula == null)
			{
				throw new NotFoundException($"La matrícula con ID {matriculaId} no existe.");

			}

			await _repository.EliminarAsync(matriculaId);
			return true;
		}

		public async Task<IEnumerable<MatriculaDto>> ObtenerMatriculaPorCursoAsync(string curso)
		{
			var matriculas = await _repository.ObtenerTodoAsync();

			var matriculasFiltradas = matriculas.Where(m => m.Curso.Nombre.ToLower().Contains(curso.ToLower()));
			return _mapper.Map<IEnumerable<MatriculaDto>>(matriculasFiltradas);
		}

		public async Task<PagedResults<MatriculaDto>> ObtenerMatriculasPaginadasAsync(MatriculaFilterDto filter)
		{
			var query = _repository.ConsultarQueryable();

			//filtrando por curso
			if (!string.IsNullOrWhiteSpace(filter.Curso))
			{
				query = query.Where(m =>m.Curso != null &&	m.Curso.Nombre.Contains(filter.Curso));
			}

			//filtrando por estudiante
			if (!string.IsNullOrWhiteSpace(filter.Estudiante))
			{
				query = query.Where(m => m.Estudiante != null && m.Estudiante.Name.Contains(filter.Estudiante));
			}

			//filtrando por nombre
			if (!string.IsNullOrWhiteSpace(filter.BusquedaPorNombre))
			{
				query = query.Where(m => m.Curso.Nombre.Contains(filter.BusquedaPorNombre));
			}
			//filtrando por estado
			if (!string.IsNullOrWhiteSpace(filter.Estado))
			{
				query = query.Where(m => m.Estado == filter.Estado);
			}

			//total de los registros
			var totalRecords = await query.CountAsync();

			var matriculas = await query.Skip((filter.Pagina - 1) * filter.RegistrosPorPagina).Take(filter.RegistrosPorPagina).ToListAsync();

			var matriculasDto = _mapper.Map<List<MatriculaDto>>(matriculas);

			return new PagedResults<MatriculaDto>
			{
				Items = matriculasDto,
				PaginaActual = filter.Pagina,
				RegistrosPorPagina = filter.RegistrosPorPagina,
				TotalRegistros = totalRecords,
				TotalPaginas = (int)Math.Ceiling((double)totalRecords / filter.RegistrosPorPagina)
			};
		}
	}
}
