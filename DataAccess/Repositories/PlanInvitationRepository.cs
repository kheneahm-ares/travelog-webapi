using DataAccess.Repositories.Interfaces;
using Domain.Models;
using Persistence;
using System;
using System.Threading.Tasks;

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

            if (invitation.InviteeId != invitee)
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
                }
            }
        }

        public async Task InviteUser(Guid inviter, Guid invitee, Guid travelPlanId)
        {
            try
            {
                //get travel plan
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                //validate the inviter is the host
                if (travelPlan.CreatedById != inviter)
                {
                    //log here
                    throw new Exception("User doesn't have rights to add to travelplan");
                }

                //validate invitee exists
                var inviteeExists = await _userRepository.DoesUserExistsAsync(invitee);
                if (!inviteeExists)
                {
                    //log here
                    throw new Exception("User to add does not exist");
                }

                var newInvitation = new PlanInvitation
                {
                    CreatedDate = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(7),
                    InvitedById = inviter,
                    InviteeId = invitee,
                    TravelPlanId = travelPlanId
                };
                _dbContext.PlanInvitations.Add(newInvitation);
                var result = await _dbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Could not add invitation in db");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}