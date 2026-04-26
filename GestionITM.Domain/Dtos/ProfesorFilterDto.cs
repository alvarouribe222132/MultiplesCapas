using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//filtro para la busqueda de profesores, este filtro se usara en el metodo de busqueda de profesores para poder paginar los resultados y ademas filtrar por nombre o por especialidad. Este tipo de clase es muy comun en las aplicaciones que necesitan mostrar datos en paginas, como por ejemplo en una tabla con paginacion. La idea es que esta clase contenga los parametros necesarios para realizar la busqueda y la paginacion, como el numero de pagina, la cantidad de registros por pagina, el filtro de busqueda por nombre y el filtro de busqueda por especialidad. Esto hace que la clase sea muy flexible y reutilizable, ya que puede ser usada con cualquier tipo de dato sin necesidad de escribir una nueva clase para cada tipo.
namespace GestionITM.Domain.Dtos
{
	public class ProfesorFilterDto //peticion o entrada para que PagedResults pueda entregar la informacion paginada (slices)
	{
		public int Pagina { get; set; } = 1; //pagina actual, por defecto es = 1
		public int RegistrosPorPagina { get; set; } = 10; //cantidad de registros por pagina, por defecto es = 10
		public string? BusquedaPorNombre { get; set; } //filtro de busqueda por nombre del profesor, es opcional (puede ser nulo)
													   //el simbolo ? significa que es opcional, es decir, que puede ser nulo. Esto es util para los filtros de busqueda, ya que no siempre se quiere filtrar por todos los campos, a veces solo se quiere filtrar por uno o por ninguno. Al hacer que estos campos sean opcionales, se le da mas flexibilidad al usuario para realizar la busqueda de profesores.
		public string? Especialidad { get; set; } //filtro de busqueda por especialidad del profesor, es opcional (puede ser nulo)
	}
}
