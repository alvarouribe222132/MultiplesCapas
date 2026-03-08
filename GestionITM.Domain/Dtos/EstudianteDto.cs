using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class EstudianteDto
	{
		public int Id { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
	}
}