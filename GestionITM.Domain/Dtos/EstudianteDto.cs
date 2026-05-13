using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionITM.Domain.Dtos
{//un DTO es un objeto que se utiliza para filtrar exactamente los datos que queremos recibir o enviar,
 //es como un molde para asegurarnos de que solo se manejen los datos necesarios y no se expongan cosas que no queremos compartir,
 //es como un filtro para controlar la información que entra y sale de nuestra aplicación
	public class EstudianteDto
	{
		public int Id { get; set; }
		public string NombreCompleto { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
	}
}