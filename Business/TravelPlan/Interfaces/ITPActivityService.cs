using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan.Interfaces
{
    public interface ITPActivityService
    {
        Task<TravelPlanActivityDto> CreateAsync(TravelPlanActivityDto activityDto, Guid userId);
        Task<TravelPlanActivityDto> EditAsync(TravelPlanActivityDto activityDto, Guid userId);
        Task<bool> DeleteAsync(Guid activityId, Guid userId);
        Task<TravelPlanActivityDto> GetAsync(Guid activityId);
        Task<List<TravelPlanActivityDto>> ListAsync(Guid travelPlanId);
    }
}
