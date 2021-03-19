using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PlanInvitationRepository : IPlanInvitationRepository
    {
        public Task<bool> InviteUser(Guid inviter, Guid invitee, Guid TravelPlanId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUser(Guid inviter, Guid invitee, Guid TravelPlanId)
        {
            throw new NotImplementedException();
        }
    }
}
