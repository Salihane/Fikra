using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Entities;

namespace Fikra.DAL
{
    public interface IDummyDataManager
    {
	    IEnumerable<Dashboard> CreateDashboards();
	    IEnumerable<Model.Entities.Task> CreateTasks();
	    DashboardTask CreateDashboardTask(string taskName);
	    Model.Entities.Task CreateTask(string taskName, int numberOfComments);
	    Project CreateProject(string projectName);
	    ProjectTask CreateProjectTask(string taskName);
	    T GetRandomEnumValue<T>();
    }
}
