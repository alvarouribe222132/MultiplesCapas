using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Interfaces
{
	public interface InterfaceProfeRepositorio
	{
		//Definimos las operaciones asincronas (Task) para manejar estudiantes  en la BD
		//Task se usa como una promesa de que las funciones se utilizaran en un futuro
		Task<IEnumerable<Profesor>> ObtenerTodoAsync(); //obtener todos los profesores 
														//IEnumerable es una coleccion generica de solo lectura. 
		Task<Profesor?> ObtenerPorIdAsync(int ProfesorId); //obtener un profesor por su ID> profesor
														   // Estudiante? tipo de referencia que acepta nulos
														   //se usa por que si se buscara un ID de estudiante que no existe, se retornaria null
														   //esto es para evitar el error null reference exception

		Task<Profesor?> ObtenerPorDocumentoAsync(string Documento);
		Task AgregarAsync(Profesor profesor); //agregar un nuevo profesor
		Task ActualizarAsync(Profesor profesor); //actualizar un profesor existente
		Task EliminarAsync(int ProfesorId); //eliminar un profesor por su ID

		Task<bool> ExistePorDocumentoAsync(string Documento); //verificar si existe un profesor por su documento de identidad

		Task<IEnumerable<Profesor>> ObtenerProfesoresPorEspecialidadAsync(string Especialidad);
	}
}
