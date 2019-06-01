using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.Model.Entities;
using Fikra.Model.Entities.Enums;
using Task = Fikra.Model.Entities.Task;

namespace Fikra.DAL
{
	public class DummyDataManager : IDummyDataManager
	{
		private static readonly Random Random = new Random(DateTime.Now.Millisecond);

		public DummyDataManager()
		{
			Mapper.Initialize(config =>
			{
				config.CreateMap<Task, ProjectTask>();
				config.CreateMap<Task, DashboardTask>();
			});
		}
		
		public IEnumerable<Dashboard> CreateDashboards()
		{
			// Home
			var homeDashboard = CreateDashboard("Home");
			homeDashboard.Tasks = new Collection<DashboardTask>
			{
				CreateDashboardTask("Painting the hall")
			};

			var plantsProject = CreateProject("Plants");
			var thabghaTask = CreateProjectTask("Thabgha");
			var thazathTask = CreateProjectTask("Thazath");
			var lewayaTask = CreateProjectTask("Lewaya");
			plantsProject.Tasks = new Collection<ProjectTask>
			{
				thabghaTask,
				thazathTask,
				lewayaTask
			};

			var toolsProject = CreateProject("Tools");
			var zakatCalculatorTask = CreateProjectTask("Zakat Calculator");
			toolsProject.Tasks = new Collection<ProjectTask>
			{
				zakatCalculatorTask
			};

			homeDashboard.Projects = new Collection<Project>
			{
				plantsProject
			};

			// Work
			var workDashboard = CreateDashboard("Work");
			workDashboard.Tasks = new Collection<DashboardTask>
			{
				CreateDashboardTask("Learning Docker"),
				CreateDashboardTask("Read Clean Architecture"),
				CreateDashboardTask("Building the Fikra app")
			};

			var agiProject = CreateProject("AGI");
			agiProject.Tasks = new Collection<ProjectTask>
			{
				CreateProjectTask("KT")
			};
			var philAtHomeProject = CreateProject("Phil At Home");
			philAtHomeProject.Tasks = new Collection<ProjectTask>
			{
				CreateProjectTask("Implementing security phase 1"),
				CreateProjectTask("Implementing security phase 2"),
				CreateProjectTask("Organize a KT session"),
				CreateProjectTask("Implementing CI/CD")
			};
			workDashboard.Projects = new Collection<Project>
			{
				agiProject,
				philAtHomeProject
			};

			// Hobbies
			var hobbiesDashboard = CreateDashboard("Hobbies");
			hobbiesDashboard.Tasks = new Collection<DashboardTask>
			{
				CreateDashboardTask("Kayak in ardennen")
			};

			return new Dashboard[] { homeDashboard, workDashboard, hobbiesDashboard };
		}

		public IEnumerable<Model.Entities.Task> CreateTasks()
		{
			var readBook = CreateTask("Read the Clean Architecture book");
			var developApp = CreateTask("Build an Angular app using .NET Core");
			var dueTask = CreateTask("A due task");
			dueTask.Due = DateTime.Now.AddDays(-1);

			return new Model.Entities.Task[] { readBook, developApp, dueTask };
		}

		private Dashboard CreateDashboard(string dashboardName)
		{
			var daysRange = Random.Next(1, 5);

			var dashboard = new Dashboard
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRange + 5)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRange + 5)),
				Name = dashboardName
			};

			return dashboard;
		}

		private DashboardTask CreateDashboardTask(string taskName)
		{
			var task = CreateTask(taskName);
			return Mapper.Map<Model.Entities.Task, DashboardTask>(task);
		}

		private Model.Entities.Task CreateTask(string taskName)
		{
			var daysRangee = Random.Next(1, 5);

			var comment = new TaskComment
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRangee + 1)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 1)),
				Value = $"This is a comment for {taskName}"
			};

			var estimate = daysRangee + 3;
			var completed = Random.Next(1, estimate);
			var remaining = estimate - completed;

			var effort = new Effort
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRangee + 2)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 2)),
				Estimated = estimate,
				Completed = completed,
				Remaining = remaining
			};

			var priority = GetRandomEnumValue<Priority>();
			var status = GetRandomEnumValue<Status>();
			var task = new Model.Entities.Task
			{
				Comments = new Collection<TaskComment> { comment },
				CreatedOn = DateTime.Now.AddDays(-(daysRangee + 3)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 3)),
				Due = DateTime.Now.AddDays(daysRangee + 3),
				Effort = effort,
				Name = taskName,
				Priority = priority,
				Status = status
			};

			return task;
		}

		private Project CreateProject(string projectName)
		{
			var daysRange = Random.Next(1, 3);

			var project = new Project
			{
				CreatedOn = DateTime.Now.AddDays(-(daysRange + 3)),
				ModifiedOn = DateTime.Now.AddDays(-(daysRange + 3)),
				Name = projectName
			};

			return project;
		}

		private ProjectTask CreateProjectTask(string taskName)
		{
			var task = CreateTask(taskName);
			return Mapper.Map<Model.Entities.Task, ProjectTask>(task);
		}

		private T GetRandomEnumValue<T>()
		{
			var values = Enum.GetValues(typeof(T));
			return (T)values.GetValue(Random.Next(values.Length));
		}
	}
}
