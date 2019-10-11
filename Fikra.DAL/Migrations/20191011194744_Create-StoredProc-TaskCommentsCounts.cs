using Fikra.DAL.StoredProcedures;
using Fikra.DAL.Types;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fikra.DAL.Migrations
{
    public partial class CreateStoredProcTaskCommentsCounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(new GuidList().Body);
			migrationBuilder.Sql(new TaskCommentsCountsProcedure().Body);

		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
