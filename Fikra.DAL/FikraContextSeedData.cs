using AutoMapper;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Entities = Fikra.Model.Entities;
using ThreadingTasks= System.Threading.Tasks;

namespace Fikra.DAL
{
    public class FikraContextSeedData
    {
        private readonly IRepository<Entities.Task, Guid> _tasksRepo;
        private readonly IRepository<Dashboard, int> _dashboardsRepo;

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public FikraContextSeedData(IRepository<Entities.Task, Guid> tasksRepo,
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

                var dueTask = CreateTask("A due task");
                dueTask.Due = DateTime.Now.AddDays(-1);
                _tasksRepo.Add(dueTask);
            }

            return await _tasksRepo.SaveChangesAsync();
        }

        private async Task<bool> CreateDashboardsAsync()
        {
			var noDashboards = await _dashboardsRepo.CountAsync() == 0;
            if (noDashboards)
            {
				// Home
                var homeDashboard = CreateDashboard("Home");
                homeDashboard.Tasks = new Collection<Entities.Task>
                {
	                CreateTask("Painting the hall")
				};

                var plantsProject = CreateProject("Plants");
                var thabghaTask = CreateTask("Thabgha");
                var thazathTask = CreateTask("Thazath");
                var lewayaTask = CreateTask("Lewaya");
                plantsProject.Tasks = new Collection<Entities.Task>
                {
	                thabghaTask,
	                thazathTask,
	                lewayaTask
                };

                var toolsProject = CreateProject("Tools");
                var zakatCalculatorTask = CreateTask("Zakat Calculator");
                toolsProject.Tasks = new Collection<Entities.Task>
                {
	                zakatCalculatorTask
                };

                homeDashboard.Projects = new Collection<Project>
                {
	                plantsProject
                };

                _dashboardsRepo.Add(homeDashboard);

				// Work
                var workDashboard = CreateDashboard("Work");
                workDashboard.Tasks = new Collection<Entities.Task>
                {
	                CreateTask("Learning Docker"),
	                CreateTask("Read Clean Architecture"),
	                CreateTask("Building the Fikra app")
                };

                var agiProject = CreateProject("AGI");
                agiProject.Tasks = new Collection<Entities.Task>
                {
	                CreateTask("KT")
                };
                var philAtHomeProject = CreateProject("Phil At Home");
                philAtHomeProject.Tasks = new Collection<Entities.Task>
                {
					CreateTask("Implementing security phase 1"),
					CreateTask("Implementing security phase 2"),
					CreateTask("Organize a KT session"),
					CreateTask("Implementing CI/CD")
                };
                workDashboard.Projects = new Collection<Project>
                {
	                agiProject,
	                philAtHomeProject
                };

                _dashboardsRepo.Add(workDashboard);

				// Hobbies
                var hobbiesDashboard = CreateDashboard("Hobbies");
                hobbiesDashboard.Tasks = new Collection<Entities.Task>
                {
	                CreateTask("Kayak in ardennen")
                };
                _dashboardsRepo.Add(hobbiesDashboard);
            }

            return await _dashboardsRepo.SaveChangesAsync();
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

        private Entities.Task CreateTask(string taskName)
        {
            var daysRangee = Random.Next(1, 5);

            var comment = new Comment
            {
                CreatedOn = DateTime.Now.AddDays(-(daysRangee + 1)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 1)),
                Content = $"This is a comment for {taskName}"
            };

            var effort = new Effort
            {
                CreatedOn = DateTime.Now.AddDays(-(daysRangee + 2)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 2)),
                Estimated = 8,
                Completed = 4,
                Remaining = 4
            };

            var task = new Entities.Task
            {
                Comments = new Collection<Comment> { comment },
                CreatedOn = DateTime.Now.AddDays(-(daysRangee + 3)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRangee + 3)),
                Due = DateTime.Now.AddDays(daysRangee + 3),
                Effort = effort,
                Name = taskName,
                Priority = Model.Entities.Enums.Priority.Medium,
                Status = Model.Entities.Enums.Status.Active
            };

            return task;
        }
    }
}
