using DataAccess.Common.Enums;
using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.TravelPlan.Interfaces
{
    public interface ITravelPlanStatusService
    {
        Task<List<TravelPlanStatusDto>> ListAsync();

        Task<TravelPlanStatusDto> GetStatusAsync(TravelPlanStatusEnum status);
    }
}