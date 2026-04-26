using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class ProfesorDto
	{
		//campos que el usuario tiene permitido enviar
		public int ProfesorId { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
		public string Especialidad {  get; set; } = string.Empty;
		public string Documento {  get; set; } = string.Empty;
	}
}