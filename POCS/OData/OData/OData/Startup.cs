using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using OData.Models;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.UriParser;

namespace OData
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

      services.AddOData();
      services
        .AddMvc(options => { options.EnableEndpointRouting = false; })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc(routeBuilder =>
      {
        routeBuilder.EnableDependencyInjection();
        routeBuilder.Filter().Count().Expand().OrderBy().Select().MaxTop(100);

        var conventions = ODataRoutingConventions.CreateDefault();
        conventions.Insert(0, new NavigationIndexRoutingConvention());
        routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel(app.ApplicationServices), new DefaultODataPathHandler(), conventions);
      });
    }

    private static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
    {
      ODataConventionModelBuilder builder = new ODataConventionModelBuilder(serviceProvider);
      builder.EntitySet<Person>(nameof(Person))
             .EntityType
             .Filter()  // Allow for the $filter Command
             .Count()   // Allow for the $count Command
             .Expand()  // Allow for the $expand Command
             .OrderBy() // Allow for the $orderby Command
             .Page()    // Allow for the $top and $skip Commands
             .Select(); // Allow for the $select Command; 
      return builder.GetEdmModel();
    }
  }

  public class NavigationIndexRoutingConvention : IODataRoutingConvention
  {
    public IEnumerable<ControllerActionDescriptor> SelectAction(RouteContext routeContext)
    {
      var cad = new ControllerActionDescriptor();
      return new List<ControllerActionDescriptor>
      {
        cad
      };
    }
  }
}
