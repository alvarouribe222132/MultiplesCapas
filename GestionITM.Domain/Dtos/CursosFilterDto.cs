using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Dtos
{
	public class CursosFilterDto
	{//antes de matricularse el estudiante necisita ver
		// los cursos disponibles
		// la cantidad de cupos disponibles para ver si si lo deja matricularse  o no
		// el nombre de profesor
		// y lo mas importante son los créditos
		public int CursoId { get; set; }
		public string NombreCurso { get; set; } = string.Empty;
		public int Creditos { get; set; }
		public int CuposDisponibles { get; set; }
		public string NombreProfesor { get; set; } = string.Empty;
	}
}
