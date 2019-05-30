using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Constants
{
	public static class ActionNames
	{
		public static class Tasks
		{
			public const string GetDashboardTasks = "GetDashboardTasks";
			public const string CreateDashboardTask = "CreateDashboardTask";
			public const string GetTaskById = "GetTaskById";
			public const string DeleteTask = "DeleteTask";
			public const string UpdateTask = "UpdateTask";
			public const string PatchTask = "PatchTask";
		}

		public static class Comments
		{
			public const string GetDashboardTaskComments = "GetDashboardTaskComments";
		}
	}
}
