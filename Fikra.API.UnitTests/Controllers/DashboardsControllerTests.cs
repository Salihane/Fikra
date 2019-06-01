using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.API.Controllers;
using Fikra.DAL;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fikra.API.UnitTests.Controllers
{
	public class DashboardsControllerTests
	{
		[Fact]
		public void GetAllDashboards_ReturnsDashboards()
		{
			// Arrange
			var fikraContext = DbContextMocker.GetFikraContext("Fikra Db");
			var dashboardsRepo = new FikraRepository<Dashboard, int>(fikraContext);
			var dashboardsController = new DashboardsController(dashboardsRepo);

			// Act
			var dashboards = dashboardsController.GetAllDashboards();

			// Assert
			Assert.NotNull(dashboards);
			Assert.True(dashboards.Any());
			fikraContext.Dispose();
		}
	}
}
