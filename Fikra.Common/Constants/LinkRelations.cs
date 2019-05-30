using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Constants
{
    public static class LinkRelations
    {
		public const string Self = "Reload";
		public const string Get = "Get";
		public const string Create = "Create";
		public const string Delete = "Delete";
		public const string Update = "Update";
		public const string Patch = "Patch";

		public static class Task
		{
			public const string CreateDashboardTask = "Create Dashboard Task";
		}
	}
}
