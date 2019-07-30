using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.API.Controllers;
using Fikra.DAL;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Fikra.API.UnitTests
{
    public class TestClass : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
	    private readonly IRepository<Dashboard, int> _dashboardRepo;
	    private readonly WebApplicationFactory<Startup> _factory;

		//public TestClass(IRepository<Dashboard, int> dashboardRepo)
		//{
		//	_dashboardRepo = dashboardRepo;
		//}

		public TestClass(WebApplicationFactory<Startup> factory)
		{
			_factory = factory;
			var client = _factory.CreateClient(new WebApplicationFactoryClientOptions());
			//var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
			//Microsoft.AspNetCore.w
		}

	    [Fact]
	    public void GetAllDashboards_ReturnsDashboards()
	    {
		    // Arrange
		    //var fikraContext = DbContextMocker.GetFikraContext("Fikra Db");
		    //var dashboardsRepo = new FikraRepository<Dashboard, int>(fikraContext);
		    var dashboardsController = new DashboardsController(_dashboardRepo);

		    // Act
		    var dashboards = dashboardsController.GetAllDashboards();

		    // Assert
		    Assert.NotNull(dashboards);
		    Assert.True(dashboards.Any());
	    }

		public void Dispose()
		{
			//_dashboardRepo.Dispose();
		}
    }
}
