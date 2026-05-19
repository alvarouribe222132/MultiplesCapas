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

namespace GestionITM.Infrastructure.Services
{
	public class CursoServices : ICursoService  //la interfaz ICursoService define los métodos que esta clase debe implementar, este se encarga de buscar todo lo neceesario para preparar todo los alimentos
	{
			private readonly InterfaceCursoRepositorio _repository;
			private readonly IMapper _mapper;

			public CursoServices(InterfaceCursoRepositorio repository,IMapper mapper)
			{
				_repository = repository;
				_mapper = mapper;
			}

			public async Task<IEnumerable<CursoDto>> ObtenerTodosLosCursosAsync()
			{
				var cursos = await _repository.ObtenerTodoAsync();

				return _mapper.Map<IEnumerable<CursoDto>>(cursos);
			}

			public async Task<CursoDto?> ObtenerPorIdAsync(int idCurso)
			{
				var curso = await _repository.ObtenerPorIdAsync(idCurso);

				if (curso == null)
				{
					throw new NotFoundException($"No existe el curso con ID {idCurso}");
				}

				return _mapper.Map<CursoDto>(curso);
			}

			public async Task<int> RegistrarCursoAsync(CursoCreateDto cursoCreateDto)
			{
				var curso = _mapper.Map<Curso>(cursoCreateDto);

				await _repository.CrearAsync(curso);

				return curso.IdCurso;
			}

			public async Task<bool> ActualizarCursoAsync(CursoUpdateDto cursoUpdateDto)
			{
				var curso = await _repository.ObtenerPorIdAsync(cursoUpdateDto.IdCurso);

				if (curso == null)
				{
					throw new NotFoundException($"No existe el curso con ID {cursoUpdateDto.IdCurso}");
				}

				_mapper.Map(cursoUpdateDto, curso);

				await _repository.ActualizarAsync(curso);

				return true;
			}

			public async Task<bool> DeleteCursoAsync(int idCurso)
			{
				var curso = await _repository.ObtenerPorIdAsync(idCurso);

				if (curso == null)
				{
					throw new NotFoundException($"No existe el curso con ID {idCurso}");
				}

				await _repository.EliminarAsync(idCurso);

				return true;
			}

		public async Task<PagedResults<CursoDto>> ObtenerCursosPaginadosAsync(CursoFilterDto filter)
		{
			//var cursos = await _repository.ObtenerTodoAsync();  eliminacion para no colapzar la BD
			//en este metodo se usa Async y await 

			var query = _repository.ConsultarQueryable(); // este Ef Core genera SQL REAL con: Where, Skip y Take ejemplo:
														  //SELECT *
														  //FROM Cursos
														  //WHERE Creditos >= 3
														  //ORDER BY IdCurso
														  //OFFSET 0 ROWS
														  //FETCH NEXT 10 ROWS ONLY

			// filtro por nombre
			if (!string.IsNullOrWhiteSpace(filter.NombreCurso))
			{
				query = query.Where(c =>
					c.Nombre.Contains(filter.NombreCurso));
			}

			// filtro por créditos
			if (filter.CreditosMinimos.HasValue)
			{
				query = query.Where(c =>
					c.Creditos >= filter.CreditosMinimos.Value);
			}

			// filtro por cupos
			if (filter.CuposMinimos.HasValue)
			{
				query = query.Where(c =>
					c.CuposDisponibles >= filter.CuposMinimos.Value);
			}

			var totalRegistros = await query.CountAsync();

			var cursosPaginados = await query
				.Skip((filter.Pagina - 1) * filter.RegistrosPorPagina)
				.Take(filter.RegistrosPorPagina)
				.ToListAsync();

			var cursosDto = _mapper.Map<List<CursoDto>>(cursosPaginados);

			return new PagedResults<CursoDto>
			{
				Items = cursosDto,
				PaginaActual = filter.Pagina,
				RegistrosPorPagina = filter.RegistrosPorPagina,
				TotalRegistros = totalRegistros,
				TotalPaginas = (int)Math.Ceiling((double)totalRegistros / filter.RegistrosPorPagina)
			};
		}


	}
}
