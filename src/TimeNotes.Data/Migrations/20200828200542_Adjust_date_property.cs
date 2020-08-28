using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations
{
    public partial class Adjust_date_property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourPointed",
                table: "TimeEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateHourPointed",
                table: "TimeEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateHourPointed",
                table: "TimeEntries");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HourPointed",
                table: "TimeEntries",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
