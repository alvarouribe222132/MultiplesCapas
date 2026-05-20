using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.AppMovil.Models
{
	public class PageResults<T>
	{
		public List<T> Items { get; set; } = new();
		public int PaginaActual { get; set; }
		public int TotalPaginas { get; set; }
		public int TotalRegistros { get; set; }
	}
}
