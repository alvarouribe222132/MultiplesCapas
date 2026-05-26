using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class ProfesorUpdateDto
	{// Este es el que usaremos para recibir los Datospublic string Nombre{ get; set; } = string.Empty;

		public int ProfesorId { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Documento { get; set; } = string.Empty;

		public string Correo { get; set; } = string.Empty;
		public string Especialidad { get; set; } = string.Empty;
		public string Telefono { get; set; } = string.Empty;
	}
}


