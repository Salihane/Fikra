using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fikra.API.Controllers;
using Fikra.API.Models;
using Fikra.DAL;
using Fikra.Model.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Fikra.API.UnitTests.Controllers
{
    public class TasksControllerTests
    {
	    [Fact]
	    public async Task Get_ForExistingDashboard_ReturnsDashboardTasks()
	    {
		    using (var fikraContext = DbContextMocker.GetFikraContext("Fikra Db"))
		    {
			    var tasksRepo = new FikraRepository<DashboardTask, Guid>(fikraContext);
				//var tasksController = new TasksController()
		    }

	    }
    }
}
