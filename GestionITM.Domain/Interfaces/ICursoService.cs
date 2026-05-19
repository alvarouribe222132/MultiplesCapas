using GestionITM.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Modelos;

namespace GestionITM.Domain.Interfaces
{
	public interface ICursoService //Este es el que habla con el Controller
	{
		Task<IEnumerable<CursoDto>> ObtenerTodosLosCursosAsync();
		Task<CursoDto?> ObtenerPorIdAsync(int IdCurso);
		Task<int> RegistrarCursoAsync(CursoCreateDto cursoDto);

		Task<bool> ActualizarCursoAsync(CursoUpdateDto cursoUpdateDto);
		Task<bool> DeleteCursoAsync(int IdCurso);
		Task<PagedResults<CursoDto>> ObtenerCursosPaginadosAsync(CursoFilterDto filter);
	}
}
