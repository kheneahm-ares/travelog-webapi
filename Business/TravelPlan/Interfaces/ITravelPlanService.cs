using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan.Interfaces
{
    public interface ITravelPlanService
    {
        Task<TravelPlanDto> CreateAsync(TravelPlanDto travelPlanDto, Guid userId);
        Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId);
        Task<Dictionary<string, TravelPlanStatusDto>> SetStatusAsync(Guid travelPlanId, Guid userId, int status);
        Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId);
        Task<bool> DeleteAsync(Guid travelPlanId, Guid userId);
        Task<TravelPlanDto> GetAsync(Guid travelPlanId, bool includeUTP = false, bool includeStatus = false);
        Task<List<TravelPlanDto>> ListAsync(Guid userId, int? status = null);
        Task<bool> RemoveTraveler(Guid loggedInUserId, string travelerUsername, Guid travelPlanId);

    }
}
