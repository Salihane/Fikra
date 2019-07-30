using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL;
using Fikra.DAL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fikra.API.IntegrationTests
{
	//Info: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2#customize-webapplicationfactory

	public class FikraWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
	{
		public IDummyDataManager DummyDataManager => _dummyDataManager;
		private IDummyDataManager _dummyDataManager;

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				var internalServiceProvider = new ServiceCollection()
						.AddEntityFrameworkInMemoryDatabase()
						.BuildServiceProvider();

				services.AddDbContext<FikraContext>(options =>
				{
					options.UseInMemoryDatabase("FikraDb");
					options.UseInternalServiceProvider(internalServiceProvider);
				});
			});

			builder.ConfigureTestServices(services =>
			{
				services.AddTransient<IDummyDataManager, DummyDataManager>();
				var serviceProvider = services.BuildServiceProvider();

				_dummyDataManager = serviceProvider.GetService<IDummyDataManager>();
			});
		}
	}
}
