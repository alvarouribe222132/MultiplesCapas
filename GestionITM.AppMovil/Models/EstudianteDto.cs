using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	public class EstudianteDto
	{
		public int Id { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
		public string Telefono { get; set; } = string.Empty;
		public string Documento { get; set; } = string.Empty;
	}
}
