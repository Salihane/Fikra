using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AutoMapper;
using Fikra.API.Controllers;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Helpers.ResourceResponse;
using Fikra.API.Mappers;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models;
using Fikra.API.Models.DashboardTask;
using Fikra.API.Services.Tasks;
using Fikra.Common.Helpers;
using Fikra.DAL;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Fikra.API.UnitTests.Controllers
{
	public class TasksControllerTests
	{
		//private IRepository<Dashboard, int> _dashboardRepo;
		//private IRepository<DashboardTask, Guid> _dashboardTasksRepo;
		//private IResourceParameters<DashboardTask, Guid> _dashboardTaskParameters;
		//private IFikraMapper<DashboardTask, DashboardTaskDto> _dashboardTaskMapper;
		//private ILinkDtoFactory<DashboardTask, Guid> _dashboardTaskLinkDtoFactory;
		//private IResourceUri<DashboardTask, Guid> _dashboardTaskUri;
		//private IResourceResultBuilder _resourceResultBuilder;
		//private IUrlHelper _urlHelper;
		//private IMapper _mapper;
		[Fact]
		public async Task Get_ForExistingDashboard_ReturnsDashboardTasks()
		{
			//using (var fikraContext = DbContextMocker.GetFikraContext("Fikra Db"))
			//{
			//	var services = new ServiceCollection();
			//	services.AddTransient<IResourceParameters<DashboardTask, Guid>, DashboardTaskResourceParameters>();
			//	services.AddTransient<IResourceUri<DashboardTask, Guid>, DashboardTaskResourceUri>();
			//	services.AddAutoMapper(typeof(Startup));
			//	services.AddTransient<IFikraMapper<DashboardTask, DashboardTaskDto>, DashboardTaskMapper>();
			//	services.AddTransient<ILinkDtoFactory<DashboardTask, Guid>, DashboardTaskLinkDtoFactory>();
			//	services.AddTransient<IResourceResultBuilder, ResourceResultBuilder>();
			//	services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			//	services.AddTransient<IUrlHelper>(factory =>
			//	{
			//		var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
			//		return new UrlHelper(actionContext);
			//	});
			//	services.AddSingleton(provider => new MapperConfiguration(config =>
			//	{
			//		config.AddProfile(new TaskProfile(provider.GetService<IConfiguration>()));
			//	}).CreateMapper());
			//	services.AddTransient<ITaskService, TaskService>();
			//	services.AddDbContext<FikraContext>(options => { options.UseInMemoryDatabase("FikraDb"); });

			//	var serviceProvider = services.BuildServiceProvider();
			//	_dashboardRepo = serviceProvider.GetService<IRepository<Dashboard, int>>();
			//	_dashboardTasksRepo = serviceProvider.GetService<IRepository<DashboardTask, Guid>>();
			//	_dashboardTaskParameters = serviceProvider.GetService<IResourceParameters<DashboardTask, Guid>>();
			//	_dashboardTaskMapper = serviceProvider.GetService<IFikraMapper<DashboardTask, DashboardTaskDto>>();
			//	_dashboardTaskLinkDtoFactory = serviceProvider.GetService<ILinkDtoFactory<DashboardTask, Guid>>();
			//	_dashboardTaskUri = serviceProvider.GetService<IResourceUri<DashboardTask, Guid>>();
			//	_resourceResultBuilder = serviceProvider.GetService<IResourceResultBuilder>();
			//	_urlHelper = serviceProvider.GetService<IUrlHelper>();
			//	_mapper = serviceProvider.GetService<IMapper>();

			//	var taskService = new TaskService(_dashboardRepo, _dashboardTasksRepo, _dashboardTaskParameters, _dashboardTaskUri, _mapper, _urlHelper, _dashboardTaskMapper, _dashboardTaskLinkDtoFactory, _resourceResultBuilder);

			//	var tasks = taskService.GetTasksByDashboardIdAsync(2, null, null);
			//	//var dashboardRepo = new FikraRepository<DashboardTask, Guid>(fikraContext);
			//	//var dashboardTasksRepo = new FikraRepository<DashboardTask, Guid>(fikraContext);

			//}

		}
	}
}
