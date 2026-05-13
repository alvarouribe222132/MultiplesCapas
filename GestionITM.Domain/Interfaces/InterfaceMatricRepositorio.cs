using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Interfaces
{
	public interface InterfaceMatricRepositorio //esta es la que habla con la BD solamente se encarga de acceder a los datos
	{
		//Definimos las operaciones asincronas (Task) para manejar las Matriculas en la BD
		//Task se usa como una promesa de que las funciones se utilizaran en un futuro
		Task<IEnumerable<Matricula>> ObtenerTodoAsync();

		Task<Matricula?> ObtenerPorIdAsync(int matriculaId);

		Task CrearAsync(Matricula matricula);

		Task ActualizarAsync(Matricula matricula);

		Task EliminarAsync(int matriculaId);

		Task<bool> ExisteMatriculaAsync(int estudianteId,int cursoId);

		IQueryable<Matricula> ConsultarQueryable();//explicar el motivo de esta función

	}


}
