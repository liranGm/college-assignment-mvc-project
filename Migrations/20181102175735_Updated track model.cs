using Microsoft.EntityFrameworkCore.Migrations;

namespace college_assignment_mvc_project.Migrations
{
    public partial class Updatedtrackmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Track",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lat",
                table: "Track",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Long",
                table: "Track",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Track",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Track");
        }
    }
}
