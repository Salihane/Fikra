using Microsoft.EntityFrameworkCore;
using System;

namespace Fikra.DAL
{
    public class FikraContext : DbContext
    {
        public DbSet<Model.Entities.Task> Tasks { get; set; }

        public FikraContext(DbContextOptions<FikraContext> options)
            : base(options) { }
    }
}
