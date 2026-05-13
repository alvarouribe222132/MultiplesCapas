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
		public Estudiante? Estudiante { get; set; }  //con el simbolo ?le digo al compilador "Esta propiedad podría quedar null" debido a que son objetos relacionados, es decir, que una matricula puede existir sin un estudiante asociado, aunque en la practica esto no deberia pasar, pero el compilador no lo sabe y por eso le digo que puede ser null.

		[Required]
		public int CursoId { get; set; }
		public Curso? Curso { get; set; } 
		public DateTime FechaMatricula { get; set; }

		[Required]
		[MaxLength(20)]
		public string Periodo { get; set; } = string.Empty;

		[MaxLength(20)]
		public string Estado { get; set; } = string.Empty;

	}
}
