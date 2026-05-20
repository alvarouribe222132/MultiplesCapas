using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionITM.Domain.Entities
{
	// las Data Annotations son instrucciones que le damos a C# sobre la propiedad que viene inmediatamente despues de declarar el []
	public class Profesor
	{
		[Key] public int ProfesorId { get; set; } //al dejar el [key] EF Core deja este campo por defecto autoincremental. es decir que en el POST no necesito ingresar un id

		[Required] //aqui le decimos que el campo no puede ser nulo ni vacio. de lo contrario la API rechazará la peticion. 
		[MaxLength(100)] 
		public string Name { get; set; } = string.Empty;

		[Required]
		[EmailAddress] //para validar que se tenga el formato correcto (@dominio)
		[MaxLength(200)]
		public string Correo { get; set; } = string.Empty;

		public DateTime FechaContratacion { get; set; } = DateTime.Now;

		[MaxLength(20)]
		public string Telefono { get; set; } = string.Empty;

		[MaxLength(20)]
		public string Documento { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Especialidad { get; set; } = string.Empty;
		public ICollection<Curso> Cursos { get; set; }
		= new List<Curso>();

	}
}
