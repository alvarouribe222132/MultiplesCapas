using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GestionITM.Infrastructure.Data;

namespace GestionITM.Infrastructure.Repositorios
{
	public class CursoRepository : ICursoRepository
	{
		private readonly ApplicationDbContext _context;

		public CursoRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Curso>> ObtenerTodoAsync()
		{
			return await _context.Cursos.ToListAsync();
		}
		public async Task<Curso?> ObtenerPorIdAsync(int IdCurso)
		{
			return await _context.Cursos.FindAsync(IdCurso);
		}
		public async Task AgregarAsync(Curso curso)
		{
			await _context.Cursos.AddAsync(curso);
			await _context.SaveChangesAsync();
		}

		public async Task ActualizarAsync(Curso curso)
		{
			_context.Cursos.Update(curso);
			await _context.SaveChangesAsync();
		}

		public async Task EliminarAsync(int IdCurso)
		{
			var curso = await _context.Cursos.FindAsync(IdCurso);
			if (curso != null)
			{
				_context.Cursos.Remove(curso);
				await _context.SaveChangesAsync();
			}
		}
	}
}
