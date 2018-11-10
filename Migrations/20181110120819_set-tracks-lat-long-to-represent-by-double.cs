using Microsoft.EntityFrameworkCore.Migrations;

namespace college_assignment_mvc_project.Migrations
{
    public partial class settrackslatlongtorepresentbydouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Long",
                table: "Track",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "Lat",
                table: "Track",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Long",
                table: "Track",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Lat",
                table: "Track",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
