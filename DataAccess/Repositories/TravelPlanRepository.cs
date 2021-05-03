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

        public TravelPlanRepository(AppDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId)
        {
            try
            {
                //check if travelplan exists
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);
                if (travelPlan == null) throw new Exception("Travel Plan Not Found");

                //check if user exists
                var userExists = await _userRepository.DoesUserExistAsync(userToAddId);
                if (!userExists) throw new Exception("Invalid User Id");

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

        public async Task<TravelPlanDto> CreateAsync(TravelPlanDto travelPlanDto, Guid userId)
        {
            try
            {
                //map here
                var newTravelPlan = new TravelPlan
                {
                    Name = travelPlanDto.Name,
                    Description = travelPlanDto.Description,
                    StartDate = travelPlanDto.StartDate,
                    EndDate = travelPlanDto.EndDate,
                    CreatedById = userId,
                    TravelPlanStatusId = (int)TravelPlanStatusEnum.Created
                };

                await _dbContext.TravelPlans.AddAsync(newTravelPlan);

                //add to jxn table
                var traveler = new UserTravelPlan
                {
                    TravelPlan = newTravelPlan,
                    UserId = userId
                };

                await _dbContext.UserTravelPlans.AddAsync(traveler);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return new TravelPlanDto(newTravelPlan);
                }
                throw new Exception("Problem Editing Travel Plan");
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid travelPlanId, Guid userId)
        {
            try
            {
                var travelPlanToDelete = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (travelPlanToDelete == null) return true;

                if (travelPlanToDelete.CreatedById != userId) throw new InsufficientRightsException("Insufficient rights to delete Travel Plan");

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

        public async Task<TravelPlanDto> EditAsync(TravelPlanDto travelPlanDto, Guid userId)
        {
            try
            {
                var travelPlanToEdit = await _dbContext.TravelPlans.FindAsync(travelPlanDto.Id);
                if (travelPlanToEdit == null) throw new Exception("Travel Plan Not Found");

                if (travelPlanToEdit.CreatedById != userId) throw new InsufficientRightsException("Insufficient rights to edit Travel Plan");

                //map here
                travelPlanToEdit.TravelPlanId = travelPlanDto.Id;
                travelPlanToEdit.Name = travelPlanDto.Name;
                travelPlanToEdit.StartDate = travelPlanDto.StartDate;
                travelPlanToEdit.EndDate = travelPlanDto.EndDate;
                travelPlanToEdit.Description = travelPlanDto.Description;

                if (!_dbContext.ChangeTracker.HasChanges()) return travelPlanDto;

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return new TravelPlanDto(travelPlanToEdit);
                }
                throw new Exception("Problem Editing Travel Plan");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanDto> GetAsync(Guid travelPlanId)
        {
            try
            {
                var travelPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (travelPlan == null) throw new Exception("Travel Plan not found");

                var travelPlanDto = new TravelPlanDto(travelPlan);
                var tpStatus = await _dbContext.TravelPlanStatuses.Where(tps => tps.UniqStatus == (TravelPlanStatusEnum)travelPlan.TravelPlanStatusId).FirstOrDefaultAsync();

                travelPlanDto.TravelPlanStatus = new TravelPlanStatusDto
                {
                    UniqStatus = tpStatus.UniqStatus,
                    Description = tpStatus.Description
                };

                return travelPlanDto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TravelPlanDto>> ListAsync(Guid userId, int? status = null)
        {
            try
            {

                //get travel plans associated with the user, whether they created it or just belong it
                var userTravelPlanIds = await _dbContext.UserTravelPlans.Where(utp => utp.UserId == userId).Select((utp) => utp.TravelPlanId).ToListAsync();

                var travelPlans = new List<TravelPlan>();

                //if null, aka not specified get all,
                //else get specific 
                if (status == null)
                {
                    travelPlans = await _dbContext.TravelPlans.Where((tp) => userTravelPlanIds.Contains(tp.TravelPlanId)).ToListAsync();
                }
                else if (Enum.IsDefined(typeof(TravelPlanStatusEnum), status))
                {
                    travelPlans = await _dbContext.TravelPlans.Where((tp) => userTravelPlanIds.Contains(tp.TravelPlanId) && tp.TravelPlanStatusId == status).ToListAsync();
                }

                List<TravelPlanDto> lstTravelPlanDto = new List<TravelPlanDto>();

                foreach(var tp in travelPlans)
                {
                    var tpDto = new TravelPlanDto(tp);
                    var tpStatus = await _dbContext.TravelPlanStatuses.Where(tps => tps.UniqStatus == (TravelPlanStatusEnum)tp.TravelPlanStatusId).FirstOrDefaultAsync();

                    tpDto.TravelPlanStatus = new TravelPlanStatusDto
                    {
                        UniqStatus = tpStatus.UniqStatus,
                        Description = tpStatus.Description
                    };

                    lstTravelPlanDto.Add(tpDto); 
                }

                return lstTravelPlanDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveTraveler(Guid loggedInUserId, string travelerUsername, Guid travelPlanId)
        {
            try
            {
                //validate travelplan
                var travelPlan = await _dbContext.TravelPlans.Where((tp) => tp.TravelPlanId == travelPlanId).Include((tp) => tp.UserTravelPlans).FirstOrDefaultAsync();

                if (travelPlan == null)
                {
                    throw new Exception("Travel Plan Not Found");
                }

                //validate traveler to remove
                var travelerToRemove = await _userRepository.GetUserAsync(travelerUsername);
                if (travelerToRemove == null)
                {
                    return true;
                }

                var userTP = travelPlan.UserTravelPlans.Where((utp) => utp.UserId.ToString() == travelerToRemove.Id).FirstOrDefault();

                //if user actually was never part of the utp then just return
                if (userTP == null)
                {
                    return true;
                }

                //only hosts have removal rights
                var isUserHost = loggedInUserId == travelPlan.CreatedById;
                var userNotHostButIsTraveler = !isUserHost && loggedInUserId.ToString() == travelerToRemove.Id;

                if (isUserHost || userNotHostButIsTraveler)
                {
                    //remove entry tying user to TP
                    _dbContext.UserTravelPlans.Remove(userTP);
                    var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                    return isSuccessful;
                }
                else
                {
                    throw new InsufficientRightsException("Insufficient rights to Travel Plan");
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}