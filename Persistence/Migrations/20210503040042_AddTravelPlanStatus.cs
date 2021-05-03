using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddTravelPlanStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TravelPlanStatusId",
                table: "TravelPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TravelPlanStatuses",
                columns: table => new
                {
                    TravelPlanStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPlanStatuses", x => x.TravelPlanStatusId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelPlanStatuses");

            migrationBuilder.DropColumn(
                name: "TravelPlanStatusId",
                table: "TravelPlans");
        }
    }
}
