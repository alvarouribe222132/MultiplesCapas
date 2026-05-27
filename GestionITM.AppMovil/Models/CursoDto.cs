using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	public class CursoDto
	{
		public int IdCurso { get; set; }
		public string Codigo { get; set; } = string.Empty;
		public string NombreCurso { get; set; } = string.Empty;
		public int Creditos { get; set; }
		public int CuposDisponibles { get; set; }
		public string NombreProfesor { get; set; } = string.Empty;
		public int ProfesorId { get; set; }
		public string Estado { get; set; } = string.Empty;
	}
}
