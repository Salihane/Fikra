using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Mappers;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fikra.API
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddDbContext<FikraFakeContext>(options => { options.UseInMemoryDatabase("FikraDb"); });

			services.AddDbContext<FikraContext>(options =>
            {
	            var configuration = new ConfigurationBuilder()
		            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
		            .AddJsonFile("appsettings.json")
		            .Build();

				var connectionString = configuration.GetConnectionString("DefaultConnection");
	            options.UseSqlServer(connectionString);
            });

            services.AddScoped(typeof(IRepository<,>), typeof(FikraRepository<,>));
            services.AddScoped(typeof(IFakeRepository<,>), typeof(FikraFakeRepository<,>));
            services.AddTransient<FikraContextSeedData>();
            services.AddTransient<FikraFakeContextSeedData>();
            services.AddTransient<IResourceUri<DashboardTask, Guid>, DashboardTaskResourceUri>();
            services.AddTransient<IResourceParameters<DashboardTask, Guid>, DashboardTaskResourceParameters>();
            services.AddSingleton<IFikraMapper<DashboardTask, DashboardTaskDto>, DashboardTaskMapper>();
            services.AddTransient<ILinkDtoFactoryOld, LinkDtoFactoryOld>();
            services.AddSingleton<IDummyDataManager, DummyDataManager>();

			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddScoped<IUrlHelper>(factory =>
			{
				var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
				return new UrlHelper(actionContext);
			});


			services.AddAutoMapper();
			services.AddSingleton(provider => new MapperConfiguration(config =>
			{
				config.AddProfile(new TaskProfile(provider.GetService<IConfiguration>()));
			}).CreateMapper());

			services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
	        IHostingEnvironment env, 
	        FikraContextSeedData dataSeeder,
	        FikraFakeContextSeedData fakeDataSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("Cors");
            app.UseHttpsRedirection();
            app.UseMvc();

            dataSeeder.EnsureSeedDataAsync().Wait();
            fakeDataSeeder.EnsureSeedDataAsync().Wait();
        }
    }
}
