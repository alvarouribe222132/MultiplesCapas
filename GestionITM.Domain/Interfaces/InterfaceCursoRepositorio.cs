using GestionITM.Domain.Entities;//aqui se usa la clase Estudiante que se encuentra en la carpeta Entities
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Interfaces
{
	public interface InterfaceCursoRepositorio //esta es la que habla con la BD
	{
		//Definimos las operaciones asincronas (Task) para manejar estudiantes  en la BD
		//Task se usa como una promesa de que las funciones se utilizaran en un futuro
		Task<IEnumerable<Curso>> ObtenerTodoAsync(); //obtener todos los cursos 
	//IEnumerable es una coleccion generica de solo lectura. 
		Task<Curso?> ObtenerPorIdAsync(int cursoId); //obtener el curso por su ID>

		Task CrearAsync(Curso curso); //agregar un nuevo curso
		Task ActualizarAsync(Curso curso); //actualizar un curso existente
		Task EliminarAsync(int cursoId); //eliminar un curso por su ID
	}
}
