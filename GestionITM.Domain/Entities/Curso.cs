using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionITM.Domain.Entities
{
	public class Curso
	{
		[Key] public int IdCurso { get; set; }

		[Required]
		[MaxLength(50)]
		public string Codigo { get; set; } = string.Empty;

		[Required]
		[MaxLength(200)]
		public string Nombre { get; set; } = string.Empty;

		// Créditos académicos del curso
		[Range(0, 30)]
		public int Creditos { get; set; }
	}
}
