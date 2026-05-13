using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class MatriculaDto
	{
		//campos que el usuario tiene permitido enviar
		//para el historial
		//para consultas moviles

		public int Id { get; set; }

		public int EstudianteId { get; set; }
		public string NombreEstudiante { get; set; } = string.Empty;
		public string NombreCurso { get; set; } = string.Empty;

		public int CursoId { get; set; }
		public DateTime FechaMatricula { get; set; }
		public string Estado { get; set; } = string.Empty;

		public string Periodo { get; set; } = string.Empty;
	}
}
