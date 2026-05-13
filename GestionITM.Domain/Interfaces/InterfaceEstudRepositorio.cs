using GestionITM.Domain.Entities;//aqui se usa la clase Estudiante que se encuentra en la carpeta Entities
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Interfaces
{
	public interface InterfaceEstudRepositorio //esta es la que habla con la BD
	{
		//Definimos las operaciones asincronas (Task) para manejar estudiantes  en la BD
		//Task se usa como una promesa de que las funciones se utilizaran en un futuro
		Task<IEnumerable<Estudiante>> ObtenerTodoAsync(); //obtener todos los estudiantes 
	//IEnumerable es una coleccion generica de solo lectura. 
		Task<Estudiante?> ObtenerPorIdAsync(int EstudianteId); //obtener un estudiante por su ID> Estudiante
// Estudiante? tipo de referencia que acepta nulos
//se usa por que si se buscara un ID de estudiante que no existe, se retornaria null
//esto es para evitar el error null reference exception

		Task AgregarAsync(Estudiante estudiante); //agregar un nuevo estudiante
		Task ActualizarAsync(Estudiante estudiante); //actualizar un estudiante existente
		Task EliminarAsync(int EstudianteId); //eliminar un estudiante por su ID

		Task<bool> ExistePorDocumentoAsync(string documento); //verificar si existe un estudiante por su documento de identidad
		Task<bool> ExistePorCorreoAsync(string correo);

	}
}
