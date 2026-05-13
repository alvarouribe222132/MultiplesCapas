using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GestionITM.Domain.Dtos
{//un DTO es un objeto que se utiliza para filtrar exactamente los datos que queremos recibir o enviar,
 //es como un molde para asegurarnos de que solo se manejen los datos necesarios y no se expongan cosas que no queremos compartir,
 //es como un filtro para controlar la información que entra y sale de nuestra aplicación
	public class ProfesorCreateDto
	{// Este es el que usaremos para recibir los Datospublic string Nombre{ get; set; } = string.Empty;
		public string NombreCompleto { get; set; } = string.Empty;
		public string Especialidad { get; set; } = string.Empty;
		public string Documento { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
	}
}


