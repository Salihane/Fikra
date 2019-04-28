using Fikra.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fikra.DAL
{
    public class FikraContext : DbContext
    {
        public DbSet<Model.Entities.Task> Tasks { get; set; }
        public DbSet<Model.Entities.TaskComment> TaskComments { get; set; }
        public DbSet<Model.Entities.DashboardTask> DashboardTasks { get; set; }
        public DbSet<Model.Entities.Dashboard> Dashboards { get; set; }

        public FikraContext(DbContextOptions<FikraContext> options)
            : base(options) { }
    }
}
