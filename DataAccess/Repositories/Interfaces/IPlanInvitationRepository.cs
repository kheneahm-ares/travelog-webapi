using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlanInvitationRepository
    {
        Task InviteUser(Guid inviter, Guid invitee, Guid TravelPlanId);
        Task AcceptInvitation(Guid invitee, int invitationId);
    }
}
