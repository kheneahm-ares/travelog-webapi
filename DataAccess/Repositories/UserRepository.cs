using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(UserDto user)
        {
            try
            {
                if(_dbContext.ApiUsers.Where(u => u.Username == user.Username).Any())
                {
                    throw new Exception("Username already exists");
                }

                var newUser = new ApiUser
                {
                    ApiUserId = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName
                };

                _dbContext.ApiUsers.Add(newUser);

                var IsSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            try
            {
                var userToDelete = await _dbContext.ApiUsers.FindAsync(userId);

                //create our own rest exception later and catch it properly
                if (userToDelete == null) throw new Exception("Not Found");

                _dbContext.ApiUsers.Remove(userToDelete);

                var IsSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditAsync(UserDto user)
        {
            try
            {
                //EF will grab from DB and store in memory, we're referencing that memory address
                var userToEdit = await _dbContext.ApiUsers.FindAsync(user.Id);

                userToEdit.DisplayName = user.DisplayName;
                userToEdit.Username = user.Username; //is this a good idea?  we have to make sure this isn't used in other tables

                //if the values are the same, just return don't hit db
                if (!_dbContext.ChangeTracker.HasChanges()) return true;

                var IsSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return IsSuccessful;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto> GetAsync(Guid userId)
        {
            try
            {
                var user = await _dbContext.ApiUsers.FindAsync(userId);

                if (user == null) throw new Exception("User Not Found");

                return new UserDto
                {
                    Id = user.ApiUserId,
                    Username = user.Username,
                    DisplayName = user.DisplayName
                };
            }
            catch
            {
                throw;
            }
        }
    }
}