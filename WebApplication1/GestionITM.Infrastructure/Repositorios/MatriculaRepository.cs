using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GestionITM.Infrastructure;
using GestionITM.Infrastructure.Data;

namespace GestionITM.Infrastructure.Repositorios
{
	public class MatriculaRepository : InterfaceMatricRepositorio
	{
		private readonly ApplicationDbContext _context;

		// Inyectamos el DbContext aqui para poder acceder a la base de datos
		public MatriculaRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		/*ApplicationDbContext
		 * en lugar de escribir sentenciar manuales como el SELECT, el INSERT, el UPDATE, etc. en la base de datos, 
		 * se utiliza el DbContext para interactuar con la base de datos de una manera mas sencilla y eficiente.
		 * este se encarga de traducir las instrucciones de C# a SQL de forma segura y optimizada, evitando errores comunes como las inyecciones SQL y mejorando el rendimiento de las consultas.
		 */

		public async Task<IEnumerable<Matricula>> ObtenerTodoAsync()
		{
			return await _context.Matriculas.Include(m => m.Estudiante).Include(m => m.Curso).ToListAsync();
			//el metodo .ToListAsync retorna una lista de todos los registros de las matriculas de la base de datos
			//y los combierte en una lista de C#
		}
		public async Task<Matricula?> ObtenerPorIdAsync(int matriculaId)
		{
			return await _context.Matriculas.Include(m => m.Estudiante).Include(m => m.Curso).FirstOrDefaultAsync(m => m.Id == matriculaId);
			//.FindAsync es un metodo ultra optimizado de EntityFrameworkCore
			//que busca la matricula por su llave primaria en este caso el ID en la base de datos
		}
		public async Task CrearAsync(Matricula matricula)
		{
			await _context.Matriculas.AddAsync(matricula);
			//.AddAsync le dice al EntityFrameworkCore "ey empiece a rastriar a una nueva matricula
			//sin embargo no lo guarde en la Base de Datos aun
			await _context.SaveChangesAsync();
			//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando INSERT
			//guarda los cambios en la base de datos
		}

		public async Task ActualizarAsync(Matricula matricula)
		{
			_context.Matriculas.Update(matricula);
			//.Update le dice al EntityFrameworkCore "ey esta matricula ya existe en la base de datos, actualizala con esta nueva informacion"
			await _context.SaveChangesAsync();
			//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando UPDATE
			//guarda los cambios en la base de datos
		}

		public async Task EliminarAsync(int matriculaId)
		{
			var matricula = await _context.Matriculas.FirstOrDefaultAsync(m => m.Id == matriculaId);

			if (matricula != null)
			{
				_context.Matriculas.Remove(matricula);
				//.Remove le dice al EntityFrameworkCore "ey esta matricula ya existe en la base de datos, eliminalo"
				await _context.SaveChangesAsync();
				//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando DELETE
				//guarda los cambios en la base de datos
			}
		}
		public async Task<bool> ExisteMatriculaAsync(int estudianteId,int cursoId)
		{
			return await _context.Matriculas.AnyAsync(m =>m.EstudianteId == estudianteId &&	m.CursoId == cursoId);
			//.AnyAsync le dice al EntityFrameworkCore "ey revisa si existe alguna matricula en la base de datos que tenga este estudiante y curso"
			//retorna true si existe, false si no existe
		}

		public IQueryable<Matricula> ConsultarQueryable()
		{
			return _context.Matriculas.Include(m => m.Estudiante).Include(m => m.Curso).AsQueryable();//explicar el motivo de esta implementacion que viene desde la InterfaceMatricRepositorio
		}
	}
}
