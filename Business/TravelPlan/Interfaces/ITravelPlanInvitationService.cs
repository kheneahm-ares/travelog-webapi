using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan.Interfaces
{
    public interface ITravelPlanInvitationService
    {
        Task InviteUser(Guid inviter, string inviteeUsername, Guid TravelPlanId);
        Task<IEnumerable<PlanInvitationDto>> List(Guid loggedInUserId);
        Task AcceptInvitation(Guid invitee, int invitationId);
        Task DeclineInvitation(Guid invitee, int invitationId);
    }
}
