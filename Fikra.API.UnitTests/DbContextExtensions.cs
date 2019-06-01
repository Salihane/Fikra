using System;
using Fikra.DAL;
using Fikra.Model.Entities;
using Xunit;

namespace Fikra.API.UnitTests
{
	public static class DbContextExtensions
	{
		public static void Seed(this FikraContext dbContext)
		{
			var dummyDataManager = new DummyDataManager();
			var dashboardsRepo = new FikraRepository<Dashboard, int>(dbContext);
			var tasksRepo = new FikraRepository<Model.Entities.Task, Guid>(dbContext);

			var tasks = dummyDataManager.CreateTasks();
			foreach (var task in tasks)
			{
				tasksRepo.Add(task);
			}
			var dashboards = dummyDataManager.CreateDashboards();
			foreach (var dashboard in dashboards)
			{
				dashboardsRepo.Add(dashboard);
			}

			dbContext.SaveChanges();
		}
	}
}
