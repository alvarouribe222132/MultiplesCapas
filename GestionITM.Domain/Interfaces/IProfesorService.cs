using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
		
	}
}
