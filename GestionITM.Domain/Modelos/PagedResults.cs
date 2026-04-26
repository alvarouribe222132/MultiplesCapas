using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//en arquitectura se llama a esto como un grapper (un envoltorio) 

namespace GestionITM.Domain.Modelos
{
	//Contenedor Generico para cualquier lista paginada. Este tipo de clase es muy comun en las aplicaciones que necesitan mostrar datos en paginas, como por ejemplo en una tabla con paginacion. La idea es que esta clase pueda contener cualquier tipo de dato (por eso es generica) y ademas tenga informacion sobre la paginacion, como el numero de pagina actual, el total de paginas, etc.
	public class PagedResults<T> //la letra T es un marcador de tipo generico (comodin), esto significa que cuando alguien use esta clase, puede especificar el tipo de dato que quiere usar en lugar de T. Por ejemplo, si alguien quiere usar esta clase para una lista de profesores, podria escribir PagedResults<Profesor>, y entonces T se reemplazaria por Profesor en toda la clase. Esto hace que la clase sea muy flexible y reutilizable, ya que puede ser usada con cualquier tipo de dato sin necesidad de escribir una nueva clase para cada tipo.
								 //funciones de la letra T:
								 //1. Reutilizacion de codigo
								 //2. Seguridad de tipos (type safety): al usar genericos, el compilador puede verificar que solo se usen los tipos correctos en tiempo de compilacion, lo que ayuda a prevenir errores de tipo en tiempo de ejecucion.
								 //3. Rendimiento o Performance: ahorra memoria y ciclos de CPU.
	{
		public List<T> Items { get; set; } = new(); // la lisa de elementos que se mostrarán en la apgina actual
		public int PaginaActual { get; set; } // El numero de pagina que se esta mostrando actualmente
		public int TotalPaginas {  get; set; }// El numero total de paginas disponibles
		public int TotalRegistros { get; set; } // El numero total de registros en la BD (sin paginar)
		public int RegistroPorPagina { get; set; } // El numero de registros que se muestran por pagina
	}
}
