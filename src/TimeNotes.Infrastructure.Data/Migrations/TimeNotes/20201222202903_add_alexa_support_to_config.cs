using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations.TimeNotes
{
    public partial class add_alexa_support_to_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseAlexaSupport",
                table: "HourPointConfigurations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseAlexaSupport",
                table: "HourPointConfigurations");
        }
    }
}
