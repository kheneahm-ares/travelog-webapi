using Business.TravelPlan.Interfaces;
using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class TravelPlanInvitationService : ITravelPlanInvitationService
    {
        private readonly IPlanInvitationRepository _planInvitationRepository;
        private readonly ITravelPlanService _travelPlanService;
        private readonly ITravelPlanRepository _travelPlanRepository;
        private readonly IUserRepository _userRepository;

        public TravelPlanInvitationService(IPlanInvitationRepository planInvitationRepository,
                                           ITravelPlanService travelPlanService, 
                                           ITravelPlanRepository travelPlanRepository,
                                           IUserRepository userRepository)
        {
            _planInvitationRepository = planInvitationRepository;
            _travelPlanService = travelPlanService;
            _travelPlanRepository = travelPlanRepository;
            _userRepository = userRepository;
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

        public async Task InviteUser(Guid inviter, string inviteeUsername, Guid travelPlanId)
        {
            try
            {
                var travelPlan = await _travelPlanRepository.GetAsync(travelPlanId, includeUTP: true);
                //validate the inviter is the host
                if (travelPlan.CreatedById != inviter)
                {
                    //log here
                    throw new InsufficientRightsException("User doesn't have rights to add to travelplan");
                }

                //validate invitee exists
                var userToInvite = await _userRepository.GetUserAsync(inviteeUsername);
                if (userToInvite == null)
                {
                    //log here
                    throw new UserNotFoundException("User to add does not exist");
                }

                //check if user to invite is already part of plan
                if (travelPlan.UserTravelPlans.Exists((utp) => utp.TravelPlanId == new Guid(userToInvite.Id)))
                {
                    throw new CommonException("User is already a traveler!");
                }

                var newInvitation = new PlanInvitation
                {
                    Created = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    InvitedById = inviter,
                    InviteeId = new Guid(userToInvite.Id),
                    TravelPlanId = travelPlanId
                };

                await _planInvitationRepository.InviteUser(newInvitation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<PlanInvitationDto>> List(Guid loggedInUserId)
        {
            try
            {
                //validate user
                var currUser = await _userRepository.GetUserAsync(loggedInUserId);
                if (currUser == null)
                {
                    //log here
                    throw new UserNotFoundException("User to add does not exist");
                }
                var userInvitations = await _planInvitationRepository.List(loggedInUserId);

                if (userInvitations == null)
                {
                    return new List<PlanInvitationDto>();
                }

                //get the inviters username
                foreach (var inv in userInvitations)
                {
                    var inviterUser = await _userRepository.GetUserAsync(inv.InvitedById);
                    if (inviterUser == null)
                    {
                        userInvitations.Remove(inv);
                    }
                    inv.InviterUsername = inviterUser.UserName;
                }
                return userInvitations;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}