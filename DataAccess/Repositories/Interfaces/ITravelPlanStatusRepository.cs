using DataAccess.Common.Enums;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanStatusRepository
    {
        Task<List<TravelPlanStatus>> ListAsync();

        Task<TravelPlanStatus> GetStatusAsync(TravelPlanStatusEnum status);
    }
}