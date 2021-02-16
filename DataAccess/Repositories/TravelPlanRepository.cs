using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TravelPlanRepository : ITravelPlanRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public TravelPlanRepository(AppDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userId)
        {
            try
            {
                //check if travelplan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);
                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                var userExists = await _userRepository.DoesUserExistsAsync(userId);
                if (!userExists) throw new Exception("Invalid User Id");

                var newUserTravelPlan = new UserTravelPlan
                {
                    UserId = userId,
                    TravelPlanId = travelPlanId
                };

                //could throw exception if traveler is already added to travel plan bc of the composite key constraint
                _dbContext.UserTravelPlans.Add(newUserTravelPlan);
                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateAsync(TravelPlanDto travelPlanDto, string userId)
        {
            try
            {
                var userGuid = new Guid(userId);
                var newTravelPlan = new TravelPlan
                {
                    Name = travelPlanDto.Name,
                    Description = travelPlanDto.Description,
                    StartDate = travelPlanDto.StartDate,
                    EndDate = travelPlanDto.EndDate,
                    CreatedById = userGuid
                };

                await _dbContext.TravelPlans.AddAsync(newTravelPlan);

                var traveler = new UserTravelPlan
                {
                    TravelPlan = newTravelPlan,
                    UserId = userGuid
                };

                await _dbContext.UserTravelPlans.AddAsync(traveler);

                var isSuccesful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccesful;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public Task<bool> Delete(Guid travelPlanId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<TravelPlan> GetAsync(Guid travelPlanId)
        {
            try
            {
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan not found");

                return travelPlan;
            }
            catch
            {
                throw;
            }
        }
    }
}