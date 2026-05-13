using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionITM.Domain.Dtos
{
	public class CursoCreateDto
	{
		[Required]
		[MaxLength(50)] 
		public string Codigo { get; set; } = string.Empty;

		[Required]
		[MaxLength(200)] 
		public string NombreCurso { get; set; } = string.Empty;
		[Range(0, 30)] 
		public int Creditos { get; set; }
	}
}
