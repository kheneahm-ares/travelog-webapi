using Dapper;
using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PlanInvitationRepository : IPlanInvitationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ITravelPlanRepository _travelPlanRepository;
        public string ConnectionString { get; }

        public PlanInvitationRepository(AppDbContext dbContext,
                                        IUserRepository userRepository,
                                        ITravelPlanRepository travelPlanRepository,
                                        IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _travelPlanRepository = travelPlanRepository;
            ConnectionString = configuration.GetConnectionString("TravelogApi");
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
            catch (DbUpdateException exc)
            {
                if(exc.InnerException is SqlException sqlExc)
                {
                    switch (sqlExc.Number)
                    {
                        case 2627: throw new UniqueConstraintException("Invitation has already been sent");
                        default: throw;
                    }
                }

            }
            catch (Exception exc)
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

                //get invitations
                const string PLAN_INVITATIONS_FOR_USER_SQL = @"SELECT INV.ID, TP.NAME as TravelPlanName, INV.INVITEEID as InviteeId, INV.INVITEDBYID as InvitedById, TP.TRAVELPLANID as TravelPlanId, INV.CREATED as CreatedDate, INV.EXPIRATION as ExpirationDate FROM PLANINVITATIONS INV INNER JOIN TRAVELPLANS TP ON TP.TRAVELPLANID = INV.TRAVELPLANID WHERE INV.INVITEEID=@loggedInUserId";

                List<PlanInvitationDto> userInvitations;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var enumerablInvs = await connection
                                                .QueryAsync<PlanInvitationDto>(PLAN_INVITATIONS_FOR_USER_SQL,
                                                                                            new { loggedInUserId = loggedInUserId });
                    userInvitations = enumerablInvs.ToList();
                }

                if(userInvitations == null)
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
            catch
            {
                throw;
            }
        }
    }
}