using DataAccess.Common.Enums;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
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

        public async Task<List<TravelPlanStatusDto>> ListAsync()
        {
            var tpStatuses = await _dbContext.TravelPlanStatuses.ToListAsync();

            var tpStatusDtos = tpStatuses.Select(tps => new TravelPlanStatusDto() { UniqStatus = tps.UniqStatus, Description = tps.Description }).ToList();
            return tpStatusDtos;
        }

        public async Task<TravelPlanStatusDto> GetStatusAsync(TravelPlanStatusEnum status)
        {
            var tpStatus = await _dbContext.TravelPlanStatuses.Where((tps) => tps.UniqStatus == status).FirstOrDefaultAsync();

            var tpStatusDto = new TravelPlanStatusDto()
            {
                Description = tpStatus.Description,
                UniqStatus = tpStatus.UniqStatus
            };

            return tpStatusDto;
        }
    }
}