using Microsoft.EntityFrameworkCore.Migrations;

namespace Pelatis.Data.Migrations
{
    public partial class Modifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tName",
                table: "Customers",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "ActiveBusiness",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveBusiness",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customers",
                newName: "tName");
        }
    }
}
