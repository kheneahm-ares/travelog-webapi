using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    ApiUserId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.ApiUserId);
                });

            migrationBuilder.CreateTable(
                name: "TravelPlans",
                columns: table => new
                {
                    TravelPlanId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlans", x => x.TravelPlanId);
                });

            migrationBuilder.CreateTable(
                name: "TravelPlanActivities",
                columns: table => new
                {
                    TravelPlanActivityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    TravelPlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlanActivities", x => x.TravelPlanActivityId);
                    table.ForeignKey(
                        name: "FK_TravelPlanActivities_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "TravelPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTravelPlans",
                columns: table => new
                {
                    TravelPlanId = table.Column<Guid>(nullable: false),
                    ApiUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTravelPlans", x => new { x.ApiUserId, x.TravelPlanId });
                    table.ForeignKey(
                        name: "FK_UserTravelPlans_ApiUsers_ApiUserId",
                        column: x => x.ApiUserId,
                        principalTable: "ApiUsers",
                        principalColumn: "ApiUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTravelPlans_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "TravelPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelPlanActivities_TravelPlanId",
                table: "TravelPlanActivities",
                column: "TravelPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTravelPlans_TravelPlanId",
                table: "UserTravelPlans",
                column: "TravelPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelPlanActivities");

            migrationBuilder.DropTable(
                name: "UserTravelPlans");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropTable(
                name: "TravelPlans");
        }
    }
}
