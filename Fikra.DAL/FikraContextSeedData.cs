﻿using AutoMapper;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using System;
using System.Threading.Tasks;
using Entities = Fikra.Model.Entities;
using ThreadingTasks = System.Threading.Tasks;

namespace Fikra.DAL
{
	public class FikraContextSeedData
	{
		private readonly IRepository<Entities.Task, Guid> _tasksRepo;
		private readonly IRepository<Dashboard, int> _dashboardsRepo;
		private readonly IDummyDataManager _dummyDataManager;

		public FikraContextSeedData(
			IRepository<Entities.Task, Guid> tasksRepo,
			IRepository<Dashboard, int> dashboardsRepo,
			IDummyDataManager dummyDataManager)
		{
			_tasksRepo = tasksRepo;
			_dashboardsRepo = dashboardsRepo;
			_dummyDataManager = dummyDataManager;
		}

		public async ThreadingTasks.Task EnsureSeedDataAsync()
		{
			await CreateTasksAsync();
			await CreateDashboardsAsync();
		}

		private async ThreadingTasks.Task CreateTasksAsync()
		{
			var noTasks = await _tasksRepo.CountAsync() == 0;
			if (!noTasks) return;

			var tasks = _dummyDataManager.CreateTasks();
			foreach (var task in tasks)
			{
				_tasksRepo.Add(task);
			}

			await _tasksRepo.SaveChangesAsync();
		}

		private async ThreadingTasks.Task CreateDashboardsAsync()
		{
			var noDashboards = await _dashboardsRepo.CountAsync() == 0;
			if (!noDashboards) return;

			var dashboards = _dummyDataManager.CreateDashboards();
			foreach (var dashboard in dashboards)
			{
				_dashboardsRepo.Add(dashboard);
			}

			await _dashboardsRepo.SaveChangesAsync();
		}
	}
}
