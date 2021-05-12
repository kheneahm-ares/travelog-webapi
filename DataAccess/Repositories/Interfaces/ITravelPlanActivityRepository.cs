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
        Task<bool> DeleteAsync(TravelPlanActivity activityToDelete);
        Task<TravelPlanActivity> GetAsync(Guid activityId);
        Task<List<TravelPlanActivity>> ListAsync(Guid travelPlanId);
    }
}
