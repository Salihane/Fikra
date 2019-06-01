using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fikra.DAL
{
	public class FikraFakeContext : DbContext
	{
		public DbSet<Model.Entities.Dashboard> Dashboards { get; set; }
		public DbSet<Model.Entities.Project> Projects { get; set; }
		public DbSet<Model.Entities.DashboardTask> DashboardTasks { get; set; }
		public DbSet<Model.Entities.ProjectTask> ProjectTasks { get; set; }
		public DbSet<Model.Entities.Effort> Efforts { get; set; }
		public DbSet<Model.Entities.TaskComment> TaskComments { get; set; }

		public FikraFakeContext(DbContextOptions<FikraFakeContext> options)
			: base(options) { }
	}
}
