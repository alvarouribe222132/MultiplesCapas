using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionITM.Domain.Entities
{
	public class Matricula
	{
		[Key] public int Id { get; set; }

		[Required]
		public int EstudianteId { get; set; }

		[Required]
		[MaxLength(50)]
		public int CursoId { get; set; }

		public DateTime FechaMatricula { get; set; }

		[Required]
		[MaxLength(20)]
		public string Periodo { get; set; } = string.Empty;

		[MaxLength(20)]
		public string Estado { get; set; } = string.Empty;

	}
}
