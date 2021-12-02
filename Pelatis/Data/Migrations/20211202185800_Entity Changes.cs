using Microsoft.EntityFrameworkCore.Migrations;

namespace Pelatis.Data.Migrations
{
    public partial class EntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ComapnyName",
                table: "Businesses",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "ActiveBusiness",
                table: "AppUsers",
                newName: "DefaultBusiness");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Businesses",
                newName: "ComapnyName");

            migrationBuilder.RenameColumn(
                name: "DefaultBusiness",
                table: "AppUsers",
                newName: "ActiveBusiness");
        }
    }
}
