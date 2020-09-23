using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations.TimeNotes
{
    public partial class Added_Start_Work_Time_Configuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartWorkTime",
                table: "HourPointConfigurations",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartWorkTime",
                table: "HourPointConfigurations");
        }
    }
}
