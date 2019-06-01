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

    }
}
