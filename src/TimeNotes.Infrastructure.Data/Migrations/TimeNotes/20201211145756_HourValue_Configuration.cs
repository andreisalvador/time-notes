using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations.TimeNotes
{
    public partial class HourValue_Configuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourValue",
                table: "HourPointConfigurations",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourValue",
                table: "HourPointConfigurations");
        }
    }
}
