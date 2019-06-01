using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL;
using Microsoft.EntityFrameworkCore;

namespace Fikra.API.UnitTests
{
	public static class DbContextMocker
	{
		public static FikraContext GetFikraContext(string dbName)
		{
			var options = new DbContextOptionsBuilder<FikraContext>()
				.UseInMemoryDatabase(dbName)
				.Options;

			var fikraContext = new FikraContext(options);
			fikraContext.Seed();

			return fikraContext;
		}
	}
}
