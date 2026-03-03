using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace GestionITM.Domain.Entities
{
    public class Estudiante
    {
		[Key] public int EstudianteId { get; set; } //al dejar el [key] EF Core deja este campo por defecto autoincremental. es decir que en el POST no necesito ingresar un id

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(200)]
        public string Correo { get; set; } = string.Empty;

        public DateTime FechaInscripcion { get; set; }

		[MaxLength(20)]
		public string Telefono { get; set; } = string.Empty;

	}
}
