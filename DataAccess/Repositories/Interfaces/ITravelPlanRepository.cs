using DataAccess.Common.Enums;
using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanRepository
    {
        Task<TravelPlan> CreateAsync(TravelPlan newTravelPlan);
        //Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId);
        Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId);
        Task<bool> DeleteAsync(TravelPlan travelPlanToDelete);
        Task<TravelPlan> GetAsync(Guid travelPlanId, bool includeUTP = false);
        Task<List<TravelPlan>> GetTravelPlansWithFilterAsync(IEnumerable<Guid> travelPlanIDs, int? status = null);
        //Task<bool> UpdateTPStatus(TravelPlan travelPlanToEdit, int status);
    }
}
