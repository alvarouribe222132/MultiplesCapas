using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.Models
{
	public class PagedResults<T>
	{
		public List<T> Items { get; set; } = new List<T>();
		public int PaginaActual { get; set; }
		public int TotalPaginas { get; set; }
		public int TotalRegistros { get; set; }
	}
}
