using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TravelPlanActivityRepository : ITravelPlanActivityRepository
    {
        private readonly AppDbContext _dbContext;

        public TravelPlanActivityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TravelPlanActivityDto> CreateAsync(TravelPlanActivityDto activityDto, Guid userId)
        {
            try
            {
                //check if TravelPlan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(activityDto.TravelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                //map here
                var newActivity = new TravelPlanActivity
                {
                    Name = activityDto.Name,
                    StartTime = activityDto.StartTime,
                    EndTime = activityDto.EndTime,
                    Category = activityDto.Category,
                    Location = new Location { 
                       Address = activityDto.Location.Address ,
                       Latitude = activityDto.Location.Latitude ,
                       Longitude = activityDto.Location.Longitude,
                    },

                    HostId = userId,
                    TravelPlanId = activityDto.TravelPlanId
                };

                _dbContext.TravelPlanActivities.Add(newActivity);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
                if (isSuccessful)
                {
                    return new TravelPlanActivityDto(newActivity);
                }
                throw new Exception("Error saving changes");
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
                var activityToDelete = await _dbContext.TravelPlanActivities.FindAsync(activityId);

                if (activityToDelete == null)
                {
                    //log maybe?
                    return true;
                }
                if (activityToDelete.HostId != userId) throw new Exception("Dont have permission to delete");

                _dbContext.TravelPlanActivities.Remove(activityToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

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
                var activityToEdit = await _dbContext.TravelPlanActivities.FindAsync(activityDto.Id);

                if (activityToEdit == null) throw new Exception("Activity not found");
                if (activityToEdit.HostId != userId) throw new Exception("Insufficient rights to edit activity");

                //map lib here
                activityToEdit.Name = activityDto.Name;
                activityToEdit.StartTime = activityDto.StartTime;
                activityToEdit.EndTime = activityDto.EndTime;
                activityToEdit.Location = new Location
                {
                    Address = activityDto.Location.Address,
                    Latitude = activityDto.Location.Latitude,
                    Longitude = activityDto.Location.Longitude,
                };
                activityToEdit.Category = activityDto.Category;

                if (!_dbContext.ChangeTracker.HasChanges())
                {
                    return activityDto;
                }

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
                var activity = await _dbContext.TravelPlanActivities.Include(tpa => tpa.Location).FirstOrDefaultAsync(tpa => tpa.TravelPlanActivityId == activityId);

                if (activity == null) throw new Exception("Activity Not Found");

                return new TravelPlanActivityDto(activity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TravelPlanActivityDto>> ListAsync(Guid travelPlanId)
        {
            try
            {
                //get all activities for a given travel plan
                var lstActivities = await _dbContext.TravelPlanActivities.Where((tpa) => tpa.TravelPlanId == travelPlanId).OrderBy(a => a.StartTime).ToListAsync();

                var lstActivityDto = lstActivities.Select((a) => new TravelPlanActivityDto(a)).ToList();

                return lstActivityDto;
            }
            catch
            {
                throw;
            }
        }
    }
}