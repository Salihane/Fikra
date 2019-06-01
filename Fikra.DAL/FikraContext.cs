using Fikra.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fikra.DAL
{
    public class FikraContext : DbContext
    {
        public DbSet<Model.Entities.Dashboard> Dashboards { get; set; }
        public DbSet<Model.Entities.Project> Projects { get; set; }
        public DbSet<Model.Entities.DashboardTask> DashboardTasks { get; set; }
        public DbSet<Model.Entities.ProjectTask> ProjectTasks { get; set; }
        public DbSet<Model.Entities.Effort> Efforts { get; set; }
        public DbSet<Model.Entities.TaskComment> TaskComments { get; set; }

        public FikraContext(DbContextOptions<FikraContext> options)
	        : base(options)
        {
        }
    }
}
