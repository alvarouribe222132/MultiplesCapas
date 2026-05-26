using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Dtos
{
	public class MatriculaUpdateDto
	{//en dado caso que el estudiante desee cancelar la matricula

		public string Estado { get; set; } = string.Empty;
		public int MatriculaId { get; set; }
		public string NombreEstudiante { get; set; } = string.Empty;
	}
}
