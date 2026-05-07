using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Data
{
	//DvContext puente entre las entidades de dominio y la base de datos
	public class ApplicationDbContext : DbContext 
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options)
		{
		}
		//Cada Dbset representa una tabla en la base de datos
		public DbSet<Estudiante> Estudiantes { get; set; }
		public DbSet<Curso> Cursos { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Matricula> Matriculas { get; set; }

		public DbSet<Profesor> Profesors { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Estudiante>().HasData(
				new Estudiante
				{
					EstudianteId = 1,
					Name = "Juan Pérez",
					Correo = "juan@itm.edu.co",
					Telefono = "3001234567",
					Documento = "1234567890",
					FechaInscripcion = new DateTime(2024, 1, 15)
				},
				new Estudiante
				{
					EstudianteId = 2,
					Name = "María García",
					Correo = "maria@itm.edu.co",
					Telefono = "3009876543",
					Documento = "0987654321",
					FechaInscripcion = new DateTime(2024, 2, 20)
				}
			);
		}
	}
}
