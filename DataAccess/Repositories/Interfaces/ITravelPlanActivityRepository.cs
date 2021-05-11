using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanActivityRepository
    {
        Task<TravelPlanActivity> CreateAsync(TravelPlanActivity newActivity);
        Task<TravelPlanActivityDto> EditAsync(TravelPlanActivityDto activityDto, Guid userId);
        Task<bool> DeleteAsync(TravelPlanActivity activityToDelete);
        Task<TravelPlanActivity> GetAsync(Guid activityId);
        Task<List<TravelPlanActivity>> ListAsync(Guid travelPlanId);
    }
}
