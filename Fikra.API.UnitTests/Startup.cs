using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Helpers.ResourceResponse;
using Fikra.API.Mappers;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models.DashboardTask;
using Fikra.API.Services.Tasks;
using Fikra.Common.Helpers;
using Fikra.DAL;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fikra.API.UnitTests
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}


		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<IResourceParameters<DashboardTask, Guid>, DashboardTaskResourceParameters>();
			services.AddTransient<IResourceUri<DashboardTask, Guid>, DashboardTaskResourceUri>();
			services.AddAutoMapper(typeof(Startup));
			services.AddTransient<IFikraMapper<DashboardTask, DashboardTaskDto>, DashboardTaskMapper>();
			services.AddTransient<ILinkDtoFactory<DashboardTask, Guid>, DashboardTaskLinkDtoFactory>();
			services.AddTransient<IResourceResultBuilder, ResourceResultBuilder>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddTransient<IUrlHelper>(factory =>
			{
				var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
				return new UrlHelper(actionContext);
			});
			services.AddSingleton(provider => new MapperConfiguration(config =>
			{
				config.AddProfile(new TaskProfile(provider.GetService<Microsoft.Extensions.Configuration.IConfiguration>()));
			}).CreateMapper());
			services.AddTransient<ITaskService, TaskService>();
			services.AddDbContext<FikraContext>(options => { options.UseInMemoryDatabase("FikraDb"); });

		}

		public void Configure(IApplicationBuilder app,
			FikraContextSeedData dataSeeder)
		{
			dataSeeder.EnsureSeedDataAsync().Wait();
		}
	}
}
