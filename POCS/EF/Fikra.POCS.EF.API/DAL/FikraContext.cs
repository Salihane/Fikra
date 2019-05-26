using Microsoft.EntityFrameworkCore;

namespace Fikra.POCS.EF.API.DAL
{
    public class FikraContext : DbContext
    {
        public DbSet<Model.Entities.Task> Tasks { get; set; }
        public DbSet<Model.Entities.Dashboard> Dashboards { get; set; }
        public DbSet<Model.Entities.Project> Projects { get; set; }

        public FikraContext(DbContextOptions<FikraContext> options)
            : base(options) { }

   //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   //     {
	  //      var connectionString =
		 //       @"Data Source=TBN00567\SQL2016C;Initial Catalog=FikraPOC;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


			//optionsBuilder.UseSqlServer(connectionString);
   //     }
	}
}
