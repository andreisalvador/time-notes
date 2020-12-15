using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations.TimeNotes
{
    public partial class Bank_of_hours_configuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankOfHours",
                table: "HourPointConfigurations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankOfHours",
                table: "HourPointConfigurations");
        }
    }
}
