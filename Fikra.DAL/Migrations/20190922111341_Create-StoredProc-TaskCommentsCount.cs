using Fikra.DAL.StoredProcedures;
using Fikra.Model.QueryEntities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fikra.DAL.Migrations
{
    public partial class CreateStoredProcTaskCommentsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(new TaskCommentsCountProcedure().Body);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
