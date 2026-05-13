using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
	public class MatriculaCreateDto
	{//un DTO es un objeto que se utiliza para filtrar exactamente los datos que queremos recibir o enviar,
	 //es como un molde para asegurarnos de que solo se manejen los datos necesarios y no se expongan cosas que no queremos compartir,
	 //es como un filtro para controlar la información que entra y sale de nuestra aplicación
		public int EstudianteId { get; set; }
		public int CursoId { get; set; }
		public string Periodo { get; set; } = string.Empty;
	}
}
