using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GestionITM.Domain.Modelos
{
	public class ErrorResponse
	{
		//Propiedades para representar la respuesta de error que se enviara al cliente
		public int  StatusCode { get; set; }
		public string Message { get; set; } = string.Empty;
		public string? Details { get; set; } //solo se llena en desarrollo para no exponer detalles del error en produccion

		public DateTime Timestamp { get; set; } = DateTime.UtcNow; // timestamp es para saber cuando ocurre un evento o novedad
	}
}
