using System;
using System.Collections.Generic;
using System.Text;
using GestionITM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext 
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options)
		{
		}

		public DbSet<Estudiante> Estudiantes { get; set; }
		public DbSet<Curso> Cursos { get; set; }
	}
}
