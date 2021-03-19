using DataAccess.Repositories.Interfaces;
using Domain.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PlanInvitationRepository : IPlanInvitationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public PlanInvitationRepository(AppDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }
        public async Task<bool> InviteUser(Guid inviter, Guid invitee, Guid travelPlanId)
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

                if(result > 0)
                {
                    //log
                    return true;
                }
                else
                {
                    throw new Exception("Could not add invitation in db");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> RemoveUser(Guid inviter, Guid invitee, Guid travelPlanId)
        {
            throw new NotImplementedException();
        }
    }
}
