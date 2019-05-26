using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fikra.POCS.EF.API.DAL;
using Fikra.POCS.EF.API.DAL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fikra.POCS.EF.API
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
			var connectionString =
				@"Data Source=TBN00567\SQL2016C;Initial Catalog=FikraPOC;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

			//services.AddDbContext<FikraContext>(options => options.UseInMemoryDatabase("fikra"));
			services.AddDbContext<FikraContext>(options => options.UseSqlServer(connectionString));
			services.AddScoped(typeof(IRepository<,>), typeof(FikraRepository<,>));
			services.AddTransient<FikraContextSeedData>();

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, FikraContextSeedData dataSeeder)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();

			dataSeeder.EnsureSeedDataAsync().Wait();
		}
	}
}
