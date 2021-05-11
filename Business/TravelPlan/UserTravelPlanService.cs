using Business.TravelPlan.Interfaces;
using DataAccess.Repositories.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class UserTravelPlanService : IUserTravelPlanService
    {
        private readonly IUserTravelPlanRepository _userTravelPlanRepository;

        public UserTravelPlanService(IUserTravelPlanRepository userTravelPlanRepository)
        {
            _userTravelPlanRepository = userTravelPlanRepository;
        }

        public async Task<bool> Delete(UserTravelPlan userTPToRemove)
        {
            try
            {
                var isSuccessful = await _userTravelPlanRepository.Delete(userTPToRemove);
                return isSuccessful;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Guid>> GetTravelersForActivityAsync(Guid travelPlanId)
        {
            try
            {

                var travelerIDs = await _userTravelPlanRepository.GetTravelersForActivityAsync(travelPlanId);
                return travelerIDs;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Guid>> GetUserTravelPlanIDsAsync(Guid userId)
        {
            try
            {
                var travelPlanIDs = await _userTravelPlanRepository.GetUserTravelPlanIDsAsync(userId);
                return travelPlanIDs;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
