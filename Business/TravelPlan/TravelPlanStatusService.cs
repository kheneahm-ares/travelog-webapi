using Business.TravelPlan.Interfaces;
using DataAccess.Common.Enums;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class TravelPlanStatusService : ITravelPlanStatusService
    {
        private readonly ITravelPlanStatusRepository _travelPlanStatusRepository;

        public TravelPlanStatusService(ITravelPlanStatusRepository travelPlanStatusRepository)
        {
            _travelPlanStatusRepository = travelPlanStatusRepository;
        }
        public async Task<TravelPlanStatusDto> GetStatusAsync(TravelPlanStatusEnum status)
        {
            var tpStatus = await _travelPlanStatusRepository.GetStatusAsync(status);
            var tpStatusDto = new TravelPlanStatusDto()
            {
                Description = tpStatus.Description,
                UniqStatus = tpStatus.UniqStatus
            };

            return tpStatusDto;
        }

        public async Task<List<TravelPlanStatusDto>> ListAsync()
        {
            var tpStatuses = await _travelPlanStatusRepository.ListAsync();

            var tpStatusDtos = tpStatuses.Select(tps => new TravelPlanStatusDto() { UniqStatus = tps.UniqStatus, Description = tps.Description }).ToList();
            return tpStatusDtos;
        }
    }
}
