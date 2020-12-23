using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeNotes.Data.Migrations
{
    public partial class add_alexa_user_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlexaUserId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlexaUserId",
                table: "AspNetUsers");
        }
    }
}
