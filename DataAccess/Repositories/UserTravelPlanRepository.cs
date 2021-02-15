using Dapper;
using DataAccess.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
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
        public string ConnectionString { get; }

        public UserTravelPlanRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("TravelogApi");
        }
        public async Task<IEnumerable<Guid>> GetTravelersForActivity(Guid travelPlanId)
        {
            const string GET_TRAVELERS_SQL = @"SELECT USERID FROM USERTRAVELPLANS WHERE TRAVELPLANID = @Id";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //use string literals and let dapper handle parameterized querying 
                var travelerIDs = await connection.QueryAsync<Guid>(GET_TRAVELERS_SQL, new { Id = travelPlanId });
                return travelerIDs;
            }
        }
    }
}
