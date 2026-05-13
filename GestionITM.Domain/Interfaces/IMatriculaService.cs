using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Modelos;

namespace GestionITM.Domain.Interfaces
{
	public interface IMatriculaService //Este es el que habla con el Controller  
	{
	//logicas de negocio
		Task<IEnumerable<MatriculaDto>> ObtenerTodasLasMatriculasAsync();
		Task<IEnumerable<MatriculaDto>> ObtenerMatriculaPorCursoAsync(string curso);
		Task<int> RegistrarMatriculaAsync(MatriculaCreateDto matriculaCreateDto);

		Task<MatriculaDto?> ObtenerPorIdAsync(int MatriculaId);

		Task<bool> ActualizarMatriculaAsync(MatriculaUpdateDto matriculaUpdateDto);
		Task<bool> DeleteMatriculaAsync(int matriculaId);

		//Nivel 5: operacion optimizada con IQueryable para paginacion y filtros avanzados
		Task<PagedResults<MatriculaDto>> ObtenerMatriculasPaginadasAsync(MatriculaFilterDto filter);

	}
}
