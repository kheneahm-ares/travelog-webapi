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

        public TravelPlanRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(TravelPlanDto travelPlanDto, string userId)
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
            }
            catch (Exception exc)
            {
            }
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