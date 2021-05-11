﻿using Domain.DTOs;
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
        Task InviteUser(Guid inviter, string inviteeUsername, Guid TravelPlanId);
        Task<PlanInvitation> GetInvitation(int invitationId);
        Task<IEnumerable<PlanInvitationDto>> List(Guid loggedInUserId);
        Task DeleteInvitation(PlanInvitation invitation);
    }
}
