using Fikra.DAL.Views;
using Fikra.Model.QueryEntities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fikra.DAL.Migrations
{
    public partial class CreateViewTaskCommentsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(new TaskCommentsCountView().Body);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
