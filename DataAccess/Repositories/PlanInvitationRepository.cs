﻿using Dapper;
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
        public async Task DeleteInvitation(PlanInvitation invitation)
        {
            try
            {
                //accepting means remove the invitation from table
                _dbContext.PlanInvitations.Remove(invitation);
                var result = await _dbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    //log here
                    throw new Exception("Could not save invitation changes");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task InviteUser(PlanInvitation newInvitation)
        {
            try
            {
                _dbContext.PlanInvitations.Add(newInvitation);
                var result = await _dbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Could not add invitation in db");
                }
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException sqlExc)
                {
                    switch (sqlExc.Number)
                    {
                        //2627 is unique id already exists
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

        public async Task<List<PlanInvitationDto>> List(Guid loggedInUserId)
        {
            try
            {
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
                return userInvitations;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PlanInvitation> GetInvitation(int invitationId)
        {
            try
            {
                var invitation = await _dbContext.PlanInvitations.FindAsync(invitationId);

                return invitation;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}