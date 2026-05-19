using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Dtos
{
	public class CursoFilterDto
	{
		public string? NombreCurso { get; set; }

		public int Pagina { get; set; } = 1;

		public int RegistrosPorPagina { get; set; } = 10;

		public int? CreditosMinimos { get; set; }

		public int? CuposMinimos { get; set; }
	}
}
