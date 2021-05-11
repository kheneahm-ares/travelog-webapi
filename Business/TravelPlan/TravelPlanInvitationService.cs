using Business.TravelPlan.Interfaces;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class TravelPlanInvitationService : ITravelPlanInvitationService
    {
        private readonly IPlanInvitationRepository _planInvitationRepository;
        private readonly ITravelPlanService _travelPlanService;

        public TravelPlanInvitationService(IPlanInvitationRepository planInvitationRepository, ITravelPlanService travelPlanService)
        {
            _planInvitationRepository = planInvitationRepository;
            _travelPlanService = travelPlanService;
        }

        public async Task AcceptInvitation(Guid invitee, int invitationId)
        {
            //get invitation
            var invitation = await _planInvitationRepository.GetInvitation(invitationId);

            if (invitation?.InviteeId != invitee)
            {
                throw new Exception("Cannot accept invitation");
            }
            var isSuccessful = await _travelPlanService.AddTravelerAsync(invitation.TravelPlanId, invitee);

            if (isSuccessful)
            {
                await _planInvitationRepository.DeleteInvitation(invitation);
            }
        }

        public async Task DeclineInvitation(Guid invitee, int invitationId)
        {
            try
            {
                var invitation = await _planInvitationRepository.GetInvitation(invitationId);

                //validate decline
                if (invitation == null)
                {
                    return;
                }
                if (invitation?.InviteeId != invitee)
                {
                    throw new Exception("Cannot delete invitation");
                }

                await _planInvitationRepository.DeleteInvitation(invitation);
            }
            catch
            {
                throw;
            }
        }

        public Task InviteUser(Guid inviter, string inviteeUsername, Guid TravelPlanId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PlanInvitationDto>> List(Guid loggedInUserId)
        {
            throw new NotImplementedException();
        }
    }
}