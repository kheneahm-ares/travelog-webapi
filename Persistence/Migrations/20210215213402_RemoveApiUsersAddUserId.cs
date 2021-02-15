using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RemoveApiUsersAddUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTravelPlans_ApiUsers_ApiUserId",
                table: "UserTravelPlans");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTravelPlans",
                table: "UserTravelPlans");

            migrationBuilder.DropColumn(
                name: "ApiUserId",
                table: "UserTravelPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserTravelPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTravelPlans",
                table: "UserTravelPlans",
                columns: new[] { "UserId", "TravelPlanId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTravelPlans",
                table: "UserTravelPlans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTravelPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "ApiUserId",
                table: "UserTravelPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTravelPlans",
                table: "UserTravelPlans",
                columns: new[] { "ApiUserId", "TravelPlanId" });

            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    ApiUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.ApiUserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTravelPlans_ApiUsers_ApiUserId",
                table: "UserTravelPlans",
                column: "ApiUserId",
                principalTable: "ApiUsers",
                principalColumn: "ApiUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
