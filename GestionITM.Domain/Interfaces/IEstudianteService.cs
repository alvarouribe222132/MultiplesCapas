using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Dtos;

namespace GestionITM.Domain.Interfaces
{
	public interface IEstudianteService
	{
		Task<IEnumerable<EstudianteDto>> ObtenerTodosLosEstudiantesAsync();
		Task<bool> RegistrarEstudianteAsync(EstudianteCreateDto estudianteDto);
	
	}
}
