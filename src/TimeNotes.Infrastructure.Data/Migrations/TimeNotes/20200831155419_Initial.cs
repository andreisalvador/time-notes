using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations.TimeNotes
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HourPointConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkDays = table.Column<int>(nullable: false),
                    OfficeHour = table.Column<TimeSpan>(nullable: false),
                    LunchTime = table.Column<TimeSpan>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourPointConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HourPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ExtraTime = table.Column<TimeSpan>(nullable: false),
                    MissingTime = table.Column<TimeSpan>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateHourPointed = table.Column<DateTime>(nullable: false),
                    HourPointsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_HourPoints_HourPointsId",
                        column: x => x.HourPointsId,
                        principalTable: "HourPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_HourPointsId",
                table: "TimeEntries",
                column: "HourPointsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourPointConfigurations");

            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "HourPoints");
        }
    }
}
