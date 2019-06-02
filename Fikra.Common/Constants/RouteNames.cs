using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Constants
{
    public static class RouteNames
    {
	    public static class Task
	    {
		    public const string TasksByDashboard = "api/dashboards/{dashboardId}/tasks";
		    public const string Tasks = "api/tasks/";
		    public const string TaskByDashboard = "api/dashboards/{dashboardId}/task";
		    public const string TaskById = "api/tasks/{id}";
	    }
    }
}
