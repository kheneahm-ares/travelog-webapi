using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlanInvitationRepository
    {
        Task<bool> InviteUser(Guid inviter, Guid invitee, Guid TravelPlanId);
        Task<bool> RemoveUser(Guid inviter, Guid invitee, Guid TravelPlanId);
    }
}
