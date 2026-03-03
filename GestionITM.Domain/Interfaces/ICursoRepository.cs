using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Interfaces
{
	public interface ICursoRepository
	{
		Task<IEnumerable<Curso>> ObtenerTodoAsync();
		Task<Curso?> ObtenerPorIdAsync(int IdCurso);

		Task AgregarAsync(Curso curso); //agregar un nuevo curso
		Task ActualizarAsync(Curso curso); //actualizar un curso existente
		Task EliminarAsync(int IdCurso); //eliminar el curso por su ID
	}
}
