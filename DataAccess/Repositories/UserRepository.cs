using Dapper;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public string ConnectionString { get; }

        public UserRepository(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("IdentityServer");
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> userIds)
        {
            const string GET_USERS_SQL = @"SELECT ID, USERNAME, DISPLAYNAME FROM ASPNETUSERS WHERE ID IN @userIds";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var userDtos = await connection.QueryAsync<UserDto>(GET_USERS_SQL, new { userIds = userIds });
                return userDtos;
            }
    
        }
    }
}
