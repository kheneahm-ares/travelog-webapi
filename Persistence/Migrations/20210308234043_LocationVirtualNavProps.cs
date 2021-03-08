using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class LocationVirtualNavProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_TravelPlanActivities_TravelPlanActivityId",
                table: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Location",
                table: "Location");

            migrationBuilder.RenameTable(
                name: "Location",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Location_TravelPlanActivityId",
                table: "Locations",
                newName: "IX_Locations_TravelPlanActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_TravelPlanActivities_TravelPlanActivityId",
                table: "Locations",
                column: "TravelPlanActivityId",
                principalTable: "TravelPlanActivities",
                principalColumn: "TravelPlanActivityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_TravelPlanActivities_TravelPlanActivityId",
                table: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_TravelPlanActivityId",
                table: "Location",
                newName: "IX_Location_TravelPlanActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Location",
                table: "Location",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_TravelPlanActivities_TravelPlanActivityId",
                table: "Location",
                column: "TravelPlanActivityId",
                principalTable: "TravelPlanActivities",
                principalColumn: "TravelPlanActivityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
