using Microsoft.EntityFrameworkCore.Migrations;

namespace college_assignment_mvc_project.Migrations
{
    public partial class AddedUserIdToGuide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Guide",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Guide");
        }
    }
}
