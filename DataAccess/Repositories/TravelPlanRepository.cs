using DataAccess.Common.Enums;
using DataAccess.CustomExceptions;
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
    public class TravelPlanRepository : ITravelPlanRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ITravelPlanStatusRepository _travelPlanStatusRepository;

        public TravelPlanRepository(AppDbContext dbContext, 
                                    IUserRepository userRepository,
                                    ITravelPlanStatusRepository travelPlanStatusRepository)

        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _travelPlanStatusRepository = travelPlanStatusRepository;
        }

        public async Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId)
        {
            try
            {
                //check if travelplan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);
                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                var newUserTravelPlan = new UserTravelPlan
                {
                    UserId = userToAddId,
                    TravelPlanId = travelPlanId
                };

                //could throw exception if traveler is already added to travel plan bc of the composite key constraint
                _dbContext.UserTravelPlans.Add(newUserTravelPlan);
                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TravelPlan> CreateAsync(TravelPlan newTravelPlan)
        {
            try
            {
                await _dbContext.TravelPlans.AddAsync(newTravelPlan);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return newTravelPlan;
                }
                throw new Exception("Problem Editing Travel Plan");
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TravelPlan travelPlanToDelete)
        {
            try
            {
                //let EF core cascade delete and delete relations with dependent tables via collection nav properties
                _dbContext.Remove(travelPlanToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId)
        //{
        //    try
        //    {
        //        var travelPlanToEdit = await _dbContext.TravelPlans.FindAsync(travelPlanDto.Id);
        //        if (travelPlanToEdit == null) throw new Exception("Travel Plan Not Found");

        //        if (travelPlanToEdit.CreatedById != userId) throw new InsufficientRightsException("Insufficient rights to edit Travel Plan");

        //        //map here
        //        travelPlanToEdit.TravelPlanId = travelPlanDto.Id;
        //        travelPlanToEdit.Name = travelPlanDto.Name;
        //        travelPlanToEdit.StartDate = travelPlanDto.StartDate;
        //        travelPlanToEdit.EndDate = travelPlanDto.EndDate;
        //        travelPlanToEdit.Description = travelPlanDto.Description;

        //        if (!_dbContext.ChangeTracker.HasChanges()) return travelPlanDto;

        //        var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

        //        if (isSuccessful)
        //        {
        //            return new TravelPlanDto(travelPlanToEdit);
        //        }
        //        throw new Exception("Problem Editing Travel Plan");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> UpdateTPStatus(TravelPlan travelPlanToEdit, int status)
        //{
        //    if (!_dbContext.ChangeTracker.HasChanges())
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
        //        return isSuccessful;
        //    }
        //}


        public async Task<TravelPlan> GetAsync(Guid travelPlanId, bool includeUTP = false)
        {
            try
            {

                TravelPlan travelPlan;
                if(includeUTP)
                {
                    travelPlan = await _dbContext.TravelPlans.Where((tp) => tp.TravelPlanId == travelPlanId)
                                                               .Include(tp => tp.UserTravelPlans)
                                                               .FirstOrDefaultAsync();
                }
                else
                {
                    travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);
                }


                var travelPlanDto = new TravelPlanDto(travelPlan);
                var tpStatus = await _dbContext.TravelPlanStatuses.Where(tps => tps.UniqStatus == (TravelPlanStatusEnum)travelPlan.TravelPlanStatusId)
                                                                  .FirstOrDefaultAsync();

                travelPlanDto.TravelPlanStatus = new TravelPlanStatusDto
                {
                    UniqStatus = tpStatus.UniqStatus,
                    Description = tpStatus.Description
                };

                return travelPlan;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TravelPlan>> GetTravelPlansWithFilterAsync(IEnumerable<Guid> travelPlanIDs, int? status = null)
        {
            var travelPlans = new List<TravelPlan>();

            //if null, aka not specified, get all,
            //else get specific 
            if (status == null)
            {
                travelPlans = await _dbContext.TravelPlans.Where((tp) => travelPlanIDs.Contains(tp.TravelPlanId))
                                                          .OrderBy((tp) => tp.StartDate).ToListAsync();
            }
            else if (Enum.IsDefined(typeof(TravelPlanStatusEnum), status))
            {
                travelPlans = await _dbContext.TravelPlans.Where((tp) => travelPlanIDs.Contains(tp.TravelPlanId) && tp.TravelPlanStatusId == status)
                                                          .OrderBy((tp) => tp.StartDate).ToListAsync();
            }

            return travelPlans;
        }
    }
}