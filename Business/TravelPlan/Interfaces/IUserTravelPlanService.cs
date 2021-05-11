using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan.Interfaces
{
    public interface IUserTravelPlanService
    {
        Task<IEnumerable<Guid>> GetTravelersForActivityAsync(Guid travelPlanId);
        Task<IEnumerable<Guid>> GetUserTravelPlanIDsAsync(Guid userId);
        Task<bool> Delete(UserTravelPlan userTPToRemove);

    }
}
