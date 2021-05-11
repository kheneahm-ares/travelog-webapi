using Dapper;
using DataAccess.Repositories.Interfaces;
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
    public class UserTravelPlanRepository : IUserTravelPlanRepository
    {
        private readonly AppDbContext _dbContext;

        public string ConnectionString { get; }

        public UserTravelPlanRepository(IConfiguration configuration, AppDbContext dbContext)
        {
            this.ConnectionString = configuration.GetConnectionString("TravelogApi");
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Guid>> GetTravelersForActivityAsync(Guid travelPlanId)
        {
            const string GET_TRAVELERS_SQL = @"SELECT USERID FROM USERTRAVELPLANS WHERE TRAVELPLANID = @Id";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //use string literals and let dapper handle parameterized querying 
                var travelerIDs = await connection.QueryAsync<Guid>(GET_TRAVELERS_SQL, new { Id = travelPlanId });
                return travelerIDs;
            }
        }

        public async Task<IEnumerable<Guid>> GetUserTravelPlanIDsAsync(Guid userId)
        {
            //get travel plans associated with the user, whether they created it or just belong it
            var userTravelPlanIds = await _dbContext.UserTravelPlans.Where(utp => utp.UserId == userId).Select((utp) => utp.TravelPlanId).ToListAsync();

            return userTravelPlanIds;
        }

        public async Task<bool> Delete(UserTravelPlan userTPToRemove)
        {
            //remove entry tying user to TP
            _dbContext.UserTravelPlans.Remove(userTPToRemove);
            var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

            return isSuccessful;
        }
    }
}
