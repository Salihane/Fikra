using System;
using System.Collections.ObjectModel;
using Fikra.POCS.EF.API.DAL.Interfaces;
using Fikra.POCS.EF.API.Model.Entities;
using ThreadingTasks = System.Threading.Tasks;

namespace Fikra.POCS.EF.API.DAL
{
	public class FikraContextSeedData
	{
		private readonly IRepository<Task, Guid> _tasksRepo;
		private readonly IRepository<Dashboard, int> _dashboardsRepo;

		private static readonly Random Random = new Random(DateTime.Now.Millisecond);

		public FikraContextSeedData(IRepository<Task, Guid> tasksRepo,
			IRepository<Dashboard, int> dashboardsRepo)
		{
			_tasksRepo = tasksRepo;
			_dashboardsRepo = dashboardsRepo;
		}

		public async ThreadingTasks.Task EnsureSeedDataAsync()
		{
			await CreateTasksAsync();
			await CreateDashboardsAsync();
		}

		private async ThreadingTasks.Task<bool> CreateTasksAsync()
		{
			var noTasks = await _tasksRepo.CountAsync() == 0;
			if (noTasks)
			{
				var readBook = CreateTask("Read the Clean Architecture book");
				_tasksRepo.Add(readBook);

				var developApp = CreateTask("Build an Angular app using .NET Core");
				_tasksRepo.Add(developApp);
			}

			return await _tasksRepo.SaveChangesAsync();
		}

		private async ThreadingTasks.Task<bool> CreateDashboardsAsync()
		{
			var noDashboards = await _dashboardsRepo.CountAsync() == 0;
			if (noDashboards)
			{
				// Home
				var homeDashboard = CreateDashboard("Home");
				homeDashboard.Tasks = new Collection<Task>
				{
					CreateTask("Painting the hall")
				};

				var plantsProject = CreateProject("Plants");
				plantsProject.Tasks = new Collection<Task>
				{
					CreateTask("Thabgha"),
					CreateTask("Lewwaya"),
					CreateTask("Thazath")
				};

				homeDashboard.Projects = new Collection<Project>
				{
					plantsProject
				};

				_dashboardsRepo.Add(homeDashboard);

				// Work
				var workDashboard = CreateDashboard("Work");
				workDashboard.Tasks = new Collection<Task>
				{
					CreateTask("Learning Docker")
				};

				var hcpsProject = CreateProject("HCPS");
				hcpsProject.Tasks = new Collection<Task>
				{
					CreateTask("Planning performance"),
					CreateTask("Disable Ctrl + V")
				};

				var attentiaProject = CreateProject("Attentia");

				var sagaProject = CreateProject("Saga");
				sagaProject.Tasks = new Collection<Task>
				{
					CreateTask("Supporting PDF format")
				};

				workDashboard.Projects = new Collection<Project>
				{
					hcpsProject,
					attentiaProject,
					sagaProject
				};
				_dashboardsRepo.Add(workDashboard);

				// Hobbies
				var hobbiesDashboard = CreateDashboard("Hobbies");
				hobbiesDashboard.Tasks = new Collection<Task>
				{
					CreateTask("Kayak in ardennen"),
					CreateTask("Completing the Fikra project")
				};
				_dashboardsRepo.Add(hobbiesDashboard);
			}

			return await _dashboardsRepo.SaveChangesAsync();
		}

		private Project CreateProject(string projectName)
		{
			var daysRang = Random.Next(1, 3);

			return new Project
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRang + 3)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRang + 3)),
				Name = projectName
			};
		}

		private Dashboard CreateDashboard(string dashboardName)
		{
			var daysRang = Random.Next(1, 5);

			var dashboard = new Dashboard
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRang + 5)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRang + 5)),
				Name = dashboardName
			};

			return dashboard;
		}

		private Task CreateTask(string taskName)
		{
			var daysRange = Random.Next(1, 5);

			var task = new Task()
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRange + 3)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRange + 3)),
				Name = taskName
			};

			return task;
		}
	}
}
