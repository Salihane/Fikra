using AutoMapper;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using FikraTask = Fikra.Model.Entities.Task;
using ThreadingTasks= System.Threading.Tasks;

namespace Fikra.DAL
{
    public class FikraContextSeedData
    {
        private readonly IRepository<FikraTask, Guid> _tasksRepo;
        private readonly IRepository<Dashboard, int> _dashboardsRepo;
        private readonly IMapper _mapper;

        private static Random Random = new Random(DateTime.Now.Millisecond);

        public FikraContextSeedData(IRepository<FikraTask, Guid> tasksRepo,
            IRepository<Dashboard, int> dashboardsRepo,
            IMapper mapper)
        {
            _tasksRepo = tasksRepo;
            _dashboardsRepo = dashboardsRepo;
            _mapper = mapper;
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

        private async ThreadingTasks.Task<bool> CreateDashboardsAsync()
        {
            var noDashboards = await _dashboardsRepo.CountAsync() == 0;
            if (noDashboards)
            {
                var homeDashboard = CreateDashboard("Home");
                homeDashboard.Tasks = new Collection<DashboardTask>
                {
                    CreateDashboardTask("Painting the hall")
                };
                _dashboardsRepo.Add(homeDashboard);

                var workDashboard = CreateDashboard("Work");
                workDashboard.Tasks = new Collection<DashboardTask>
                {
                    CreateDashboardTask("Learning Docker")
                };
                _dashboardsRepo.Add(workDashboard);

                var hobbiesDashboard = CreateDashboard("Hobbies");
                hobbiesDashboard.Tasks = new Collection<DashboardTask>
                {
                    CreateDashboardTask("Kayak in ardennen"),
                    CreateDashboardTask("Completing the Fikra project")
                };
                _dashboardsRepo.Add(hobbiesDashboard);
            }

            return await _dashboardsRepo.SaveChangesAsync();
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

        private FikraTask CreateTask(string taskName)
        {
            var daysRange = Random.Next(1, 5);

            var comment = new TaskComment
            {
                CreatedOn = DateTime.Now.AddDays(-(daysRange + 1)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRange + 1)),
                Content = $"This is a comment for {taskName}"
            };

            var effort = new TaskEffort
            {
                CreatedOn = DateTime.Now.AddDays(-(daysRange + 2)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRange + 2)),
                Estimated = 8,
                Completed = 4,
                Remaining = 4
            };

            var task = new FikraTask
            {
                Comments = new Collection<TaskComment> { comment },
                CreatedOn = DateTime.Now.AddDays(-(daysRange + 3)),
                ModifiedOn = DateTime.Now.AddDays(-(daysRange + 3)),
                Due = DateTime.Now.AddDays(daysRange + 3),
                Effort = effort,
                Name = taskName,
                Priority = Model.Entities.Enums.Priority.Medium,
                Status = Model.Entities.Enums.Status.Active
            };

            return task;
        }

        private DashboardTask CreateDashboardTask(string taskName)
        {
            var task = CreateTask(taskName);
            return _mapper.Map<DashboardTask>(task);
        }
    }
}
