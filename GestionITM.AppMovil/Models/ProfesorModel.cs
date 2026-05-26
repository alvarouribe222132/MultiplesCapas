using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	// Solo propiedades puras, cero lógica visual
	public class ProfesorModel
	{
		public int ProfesorId { get; set; }

		public string NombreCompleto { get; set; } = string.Empty;

		public string Correo { get; set; } = string.Empty;

		public string Especialidad { get; set; } = string.Empty;

		public string Documento { get; set; } = string.Empty;

		public string Telefono { get; set; } = string.Empty;
	}
}