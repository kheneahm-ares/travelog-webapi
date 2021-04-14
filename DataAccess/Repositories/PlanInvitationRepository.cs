using DataAccess.Repositories.Interfaces;
using Domain.Models;
using Persistence;
using System;
using System.Threading.Tasks;
using DataAccess.CustomExceptions;

namespace DataAccess.Repositories
{
    public class PlanInvitationRepository : IPlanInvitationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ITravelPlanRepository _travelPlanRepository;

        public PlanInvitationRepository(AppDbContext dbContext,
                                        IUserRepository userRepository,
                                        ITravelPlanRepository travelPlanRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _travelPlanRepository = travelPlanRepository;
        }

        public async Task AcceptInvitation(Guid invitee, int invitationId)
        {
            //get invitation
            var invitation = await _dbContext.PlanInvitations.FindAsync(invitationId);

            if (invitation?.InviteeId != invitee)
            {
                throw new Exception("Cannot accept invitation");
            }

            var isSuccessful = await _travelPlanRepository.AddTravelerAsync(invitation.TravelPlanId, invitee);

            if (isSuccessful)
            {
                //after we added the user, remove the invitation
                _dbContext.PlanInvitations.Remove(invitation);
                var result = await _dbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    //log here
                    throw new Exception("Could not save invitation changes");
                }
            }
        }

        public async Task DeclineInvitation(Guid invitee, int invitationId)
        {
            var invitation = await _dbContext.PlanInvitations.FindAsync(invitationId);

            if (invitation == null)
            {
                return;
            }
            if (invitation?.InviteeId != invitee)
            {
                throw new Exception("Cannot delete invitation");
            }

            //remove invitation from table
            _dbContext.PlanInvitations.Remove(invitation);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
            {
                //log here
                throw new Exception("Could not save invitation changes");
            }
        }

        public async Task InviteUser(Guid inviter, string inviteeUsername, Guid travelPlanId)
        {
            try
            {
                //get travel plan
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

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

                var newInvitation = new PlanInvitation
                {
                    Created = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    InvitedById = inviter,
                    InviteeId = new Guid(userToInvite.Id),
                    TravelPlanId = travelPlanId
                };
                _dbContext.PlanInvitations.Add(newInvitation);
                var result = await _dbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Could not add invitation in db");
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}