using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanActivityRepository
    {
        Task<bool> CreateAsync(TravelPlanActivityDto activityDto, Guid userId);
        Task<TravelPlanActivityDto> EditAsync(TravelPlanActivityDto activityDto, Guid userId);
        Task<bool> DeleteAsync(Guid activityId, Guid userId);
        Task<TravelPlanActivityDto> GetAsync(Guid activityId);
        Task<List<TravelPlanActivityDto>> ListAsync(Guid travelPlanId);
    }
}
