using DataAccess.Common.Enums;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TravelPlanStatusRepository : ITravelPlanStatusRepository
    {
        private readonly AppDbContext _dbContext;

        public TravelPlanStatusRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TravelPlanStatus>> ListAsync()
        {
            var tpStatuses = await _dbContext.TravelPlanStatuses.ToListAsync();
            return tpStatuses;
        }

        public async Task<TravelPlanStatus> GetStatusAsync(TravelPlanStatusEnum status)
        {
            var tpStatus = await _dbContext.TravelPlanStatuses.Where((tps) => tps.UniqStatus == status).FirstOrDefaultAsync();
            return tpStatus;

        }
    }
}