using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	public class MatriculaDto
	{
		public int Id { get; set; }
		public string NombreEstudiante { get; set; } = string.Empty;
		public string NombreCurso { get; set; } = string.Empty;
		public string Estado { get; set; } = string.Empty;
		public string Periodo { get; set; } = string.Empty;
	}
}
