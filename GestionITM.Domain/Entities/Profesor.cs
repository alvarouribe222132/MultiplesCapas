using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionITM.Domain.Entities
{
	public class Profesor
	{
		[Key] public int ProfesorId { get; set; } //al dejar el [key] EF Core deja este campo por defecto autoincremental. es decir que en el POST no necesito ingresar un id

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[EmailAddress]
		[MaxLength(200)]
		public string Correo { get; set; } = string.Empty;

		public DateTime FechaContratacion { get; set; }

		[MaxLength(20)]
		public string Telefono { get; set; } = string.Empty;

		[MaxLength(20)]
		public string Documento { get; set; } = string.Empty;

		public string Especialidad { get; set; } = string.Empty;
		[Required]

	}
}
