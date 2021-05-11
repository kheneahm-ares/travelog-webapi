﻿using DataAccess.CustomExceptions;
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

        public async Task<TravelPlanActivity> CreateAsync(TravelPlanActivity newActivity)
        {
            try
            {
                _dbContext.TravelPlanActivities.Add(newActivity);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
                if (isSuccessful)
                {
                    return newActivity;
                }
                throw new Exception("Error saving changes");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TravelPlanActivity activityToDelete)
        {
            try
            {
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
                var activityToEdit = await _dbContext.TravelPlanActivities
                                                     .Include(tpa => tpa.Location)
                                                     .FirstOrDefaultAsync(tpa => tpa.TravelPlanActivityId == activityDto.Id);

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

        public async Task<TravelPlanActivity> GetAsync(Guid activityId)
        {
            try
            {
                var activity = await _dbContext.TravelPlanActivities.Include(tpa => tpa.Location).FirstOrDefaultAsync(tpa => tpa.TravelPlanActivityId == activityId);

                if (activity == null) throw new Exception("Activity Not Found");

                return activity;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TravelPlanActivity>> ListAsync(Guid travelPlanId)
        {
            try
            {
                //get all activities for a given travel plan
                var lstActivities = await _dbContext.TravelPlanActivities
                                            .Include(tpa => tpa.Location)
                                            .Where((tpa) => tpa.TravelPlanId == travelPlanId)
                                            .OrderBy(a => a.StartTime).ToListAsync();


                return lstActivities;
            }
            catch
            {
                throw;
            }
        }
    }
}