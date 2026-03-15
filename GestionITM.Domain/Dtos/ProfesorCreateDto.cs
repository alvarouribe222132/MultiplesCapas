using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GestionITM.Domain.Dtos
{
	public class ProfesorCreateDto
	{// Este es el que usaremos para recibir los Datospublic string Nombre{ get; set; } = string.Empty;
		public string NombreCompleto { get; set; } = string.Empty;
		public string Especialidad { get; set; } = string.Empty;
		public string Documento { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
	}
}


