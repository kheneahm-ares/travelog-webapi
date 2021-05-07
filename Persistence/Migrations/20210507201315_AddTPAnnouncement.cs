using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddTPAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPAnnouncements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    TravelPlanId = table.Column<Guid>(nullable: false),
                    TravelPlanActivityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPAnnouncements_TravelPlanActivities_TravelPlanActivityId",
                        column: x => x.TravelPlanActivityId,
                        principalTable: "TravelPlanActivities",
                        principalColumn: "TravelPlanActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPAnnouncements_TravelPlans_TravelPlanId",
                        column: x => x.TravelPlanId,
                        principalTable: "TravelPlans",
                        principalColumn: "TravelPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TPAnnouncements_TravelPlanActivityId",
                table: "TPAnnouncements",
                column: "TravelPlanActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_TPAnnouncements_TravelPlanId",
                table: "TPAnnouncements",
                column: "TravelPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPAnnouncements");
        }
    }
}
