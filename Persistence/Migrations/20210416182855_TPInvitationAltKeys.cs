using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class TPInvitationAltKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PlanInvitations_InvitedById_InviteeId_TravelPlanId",
                table: "PlanInvitations",
                columns: new[] { "InvitedById", "InviteeId", "TravelPlanId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PlanInvitations_InvitedById_InviteeId_TravelPlanId",
                table: "PlanInvitations");
        }
    }
}
