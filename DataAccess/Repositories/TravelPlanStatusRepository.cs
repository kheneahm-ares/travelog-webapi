using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<List<TravelPlanStatusDto>> List()
        {
            var tpStatuses = await _dbContext.TravelPlanStatuses.ToListAsync();

            var tpStatusDtos = tpStatuses.Select(tps => new TravelPlanStatusDto(){ UniqStatus = tps.UniqStatus, Description = tps.Description }).ToList();
            return tpStatusDtos;
        }
    }
}
