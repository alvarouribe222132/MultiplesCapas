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
	}
}
