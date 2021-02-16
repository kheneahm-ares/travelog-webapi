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
        Task<bool> CreateAsync(TravelPlanDto travelPlanDto, string userId); 
        Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userId);
        Task<bool> Delete(Guid travelPlanId, Guid userId);
        Task<TravelPlan> GetAsync(Guid travelPlanId);
    }
}
