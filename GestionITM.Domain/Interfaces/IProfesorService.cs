using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Modelos;

namespace GestionITM.Domain.Interfaces
{
	public interface IProfesorService //Este es el que habla con el Controller
	{
		Task<IEnumerable<ProfesorDto>> ObtenerTodosLosProfesoresAsync();

		Task<IEnumerable<ProfesorDto>> ObtenerProfesoresPorEspecialidadAsync(string Especialidad);

		Task<ProfesorDto?> ObtenerPorDocumentoAsync(string documento);

		Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto);

		Task<ProfesorDto?> ObtenerPorIdAsync(int ProfesorId);

		Task<bool> ActualizarProfesorAsync(ProfesorUpdateDto profesorUpdateDto);
		Task<bool> DeleteProfesorAsync(int profesorId);

		//Nivel 5: operacion optimizada con IQueryable para paginacion y filtros avanzados
		Task<PagedResults<ProfesorDto>> ObtenerProfesoresPaginadosAsync(ProfesorFilterDto filter);

	}
}
