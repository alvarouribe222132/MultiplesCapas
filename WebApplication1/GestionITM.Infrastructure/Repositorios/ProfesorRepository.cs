using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GestionITM.Infrastructure.Data;

/*EntityFrameworkCore es el ORM (Object-Relational Mapper) oficial de Microsoft para .NET que permite las interacciones con bases de datos relacionales utilizando objetos de C#.
 * es el encargado de crear las Tablas, las llaves primarias, las relaciones entre tablas, de hacer los INSERT, los SELECT, los UPDATE, los DELETE, etc. en la base de datos.
 * este es el encargado del SQLServer
 */

namespace GestionITM.Infrastructure.Repositorios
{
	public class ProfesorRepository : InterfaceProfeRepositorio //llamando a la interfaz de estudiante para implementar TODOS los metodos que se van a usar en el controlador. si no se utilizan todos va a causar problemas
	{
		//la clase EstudianteRepository es como si analogicamente fuera el Chef de la cosina el cual es el encargado
		//de saber hacer los platos. por consiguiente debe conectarse a la base de datos para obtener los ingredientes (datos) necesarios para preparar los platos (respuestas a las solicitudes del controlador)
		//Constructor 
		private readonly ApplicationDbContext _context;

		// Inyectamos el DbContext aqui para poder acceder a la base de datos
		public ProfesorRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/*ApplicationDbContext
		 * en lugar de escribir sentenciar manuales como el SELECT, el INSERT, el UPDATE, etc. en la base de datos, 
		 * se utiliza el DbContext para interactuar con la base de datos de una manera mas sencilla y eficiente.
		 * este se encarga de traducir las instrucciones de C# a SQL de forma segura y optimizada, evitando errores comunes como las inyecciones SQL y mejorando el rendimiento de las consultas.
		 */

		public async Task<IEnumerable<Profesor>> ObtenerTodoAsync()
		{
			return await _context.Profesors.ToListAsync();
			//el metodo .ToListAsync retorna una lista de todos los registros de estudiantes de la base de datos
			//y los combierte en una lista de C#
		}
		public async Task<Profesor?> ObtenerPorIdAsync(int ProfesorId)
		{
			return await _context.Profesors.FindAsync(ProfesorId);
			//.FindAsync es un metodo ultra optimizado de EntityFrameworkCore
			//que busca un estudiante por su llave primaria en este caso el ID en la base de datos
		}
		public async Task AgregarAsync(Profesor profesor)
		{
			await _context.Profesors.AddAsync(profesor);
			//.AddAsync le dice al EntityFrameworkCore "ey empiece a rastriar a este nuevo estudiante
			//sin embargo no lo guarde en la Base de Datos aun
			await _context.SaveChangesAsync();
			//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando INSERT
			//guarda los cambios en la base de datos
		}

		public async Task ActualizarAsync(Profesor profesor)
		{
			_context.Profesors.Update(profesor);
			//.Update le dice al EntityFrameworkCore "ey este estudiante ya existe en la base de datos, actualizalo con esta nueva informacion"
			await _context.SaveChangesAsync();
			//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando UPDATE
			//guarda los cambios en la base de datos
		}

		public async Task EliminarAsync(int ProfesorId)
		{
			var profesor = await _context.Profesors.FindAsync(ProfesorId);
			if (profesor != null)
			{
				_context.Profesors.Remove(profesor);
				//.Remove le dice al EntityFrameworkCore "ey este estudiante ya existe en la base de datos, eliminalo"
				await _context.SaveChangesAsync();
				//.SaveChangesAsync le dice al EntityFrameworkCore  ejecuta el comando DELETE
				//guarda los cambios en la base de datos
			}
		}

		public async Task<bool> ExistePorDocumentoAsync(string documento)
		{
			return await _context.Profesors.AnyAsync(e => e.Documento == documento);
			//.AnyAsync le dice al EntityFrameworkCore "ey revisa si existe algun estudiante en la base de datos que tenga este documento"
			//retorna true si existe, false si no existe
		}

		public async Task<IEnumerable<Profesor>> ObtenerProfesoresPorEspecialidadAsync(string Especialidad)
		{
			return await _context.Profesors
				.Where(p => p.Especialidad == Especialidad)   //esto seria como un Select en SQl donde se filtra por el tipo de especialidad
				.ToListAsync();
		}


		public async Task<Profesor?> ObtenerPorDocumentoAsync(string Documento)
		{
			return await _context.Profesors.FirstOrDefaultAsync(P => P.Documento == Documento);
			//aqui FirstOrDefaultAsync filtra la primera coincidencia con el documento 
			//si no encuentra ninguno retornara NUll por eso la instruccion Task<Profesor?> tiene el simbolo ?
			//es como si se hiciera un Select Top 1 * From Profesors Where Documento = 'xxxx'
		}
	}
}