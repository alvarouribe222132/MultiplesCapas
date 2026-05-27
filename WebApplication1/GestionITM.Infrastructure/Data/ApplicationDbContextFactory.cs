using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GestionITM.Infrastructure.Data
{
	internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder =
				new DbContextOptionsBuilder<ApplicationDbContext>();

			optionsBuilder.UseSqlServer(
				"Server=localhost;Database=GestionITMb;Trusted_Connection=True;TrustServerCertificate=True");

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}

}
