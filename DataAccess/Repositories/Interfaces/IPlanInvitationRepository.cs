using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlanInvitationRepository
    {
        Task InviteUser(PlanInvitation newInvitation);
        Task<PlanInvitation> GetInvitation(int invitationId);
        Task<List<PlanInvitationDto>> List(Guid loggedInUserId);
        Task DeleteInvitation(PlanInvitation invitation);
    }
}
