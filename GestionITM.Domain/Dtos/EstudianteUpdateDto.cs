using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class EstudianteUpdateDto
	{// Este es el que usaremos para recibir los Datospublic string Nombre{ get; set; } = string.Empty;

		public int EstudianteId { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
		public string Documento { get; set; } = string.Empty;
		public string Telefono { get; set; } = string.Empty;
	}
}


