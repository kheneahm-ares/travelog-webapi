using Business.TravelPlan.Interfaces;
using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class TPActivityService : ITPActivityService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITravelPlanService _travelPlanService;
        private readonly ITravelPlanActivityRepository _travelPlanActivityRepository;

        public TPActivityService(AppDbContext dbContext, ITravelPlanService travelPlanService, ITravelPlanActivityRepository travelPlanActivityRepository)
        {
            _dbContext = dbContext;
            _travelPlanService = travelPlanService;
            _travelPlanActivityRepository = travelPlanActivityRepository;
        }

        public async Task<TravelPlanActivityDto> CreateAsync(TravelPlanActivityDto activityDto, Guid userId)
        {
            try
            {
                var travelPlan = await _travelPlanService.GetAsync(activityDto.TravelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                //map here
                var newActivity = new TravelPlanActivity
                {
                    Name = activityDto.Name,
                    StartTime = activityDto.StartTime,
                    EndTime = activityDto.EndTime,
                    Category = activityDto.Category,
                    Location = new Location
                    {
                        Address = activityDto.Location.Address,
                        Latitude = activityDto.Location.Latitude,
                        Longitude = activityDto.Location.Longitude,
                    },
                    HostId = userId,
                    TravelPlanId = activityDto.TravelPlanId
                };

                var addedActivity = await _travelPlanActivityRepository.CreateAsync(newActivity);

                return new TravelPlanActivityDto(addedActivity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid activityId, Guid userId)
        {
            try
            {
                var activityToDelete = await _travelPlanActivityRepository.GetAsync(activityId);
                if (activityToDelete == null)
                {
                    //log maybe?
                    return true;
                }
                if (activityToDelete.HostId != userId) throw new InsufficientRightsException("Insufficient rights to delete activity");

                var isSuccessful = await _travelPlanActivityRepository.DeleteAsync(activityToDelete);

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanActivityDto> EditAsync(TravelPlanActivityDto activityDto, Guid userId)
        {
            try
            {
                var activityToEdit = await _travelPlanActivityRepository.GetAsync(activityDto.Id);

                if (activityToEdit == null) throw new Exception("Activity not found");
                if (activityToEdit.HostId != userId) throw new InsufficientRightsException("Insufficient rights to edit activity");

                //map lib here
                activityToEdit.Name = activityDto.Name;
                activityToEdit.StartTime = activityDto.StartTime;
                activityToEdit.EndTime = activityDto.EndTime;
                activityToEdit.Location.Address = activityDto.Location.Address;
                activityToEdit.Location.Longitude = activityDto.Location.Longitude;
                activityToEdit.Location.Latitude = activityDto.Location.Latitude;
                activityToEdit.Category = activityDto.Category;

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
                if (isSuccessful)
                {
                    return new TravelPlanActivityDto(activityToEdit);
                }
                throw new Exception("Error saving changes");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanActivityDto> GetAsync(Guid activityId)
        {
            try
            {
                var activity = await _travelPlanActivityRepository.GetAsync(activityId);
                return new TravelPlanActivityDto(activity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TravelPlanActivityDto>> ListAsync(Guid travelPlanId)
        {
            try
            {
                var lstActivities = await _travelPlanActivityRepository.ListAsync(travelPlanId);
                var lstActivityDto = lstActivities.Select((a) => new TravelPlanActivityDto(a)).ToList();

                return lstActivityDto;
            }
            catch (Exception)
            {
                throw;
            };
        }
    }
}