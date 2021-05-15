using Business.TravelPlan.Interfaces;
using DataAccess.Common.Enums;
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
    public class TravelPlanService : ITravelPlanService
    {
        private readonly ITravelPlanRepository _travelPlanRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTravelPlanService _userTravelPlanService;
        private readonly ITravelPlanStatusService _travelPlanStatusService;
        private readonly AppDbContext _dbContext;

        public TravelPlanService(ITravelPlanRepository travelPlanRepository,
                                 IUserRepository userRepository,
                                 IUserTravelPlanService userTravelPlanService,
                                 ITravelPlanStatusService travelPlanStatusService,
                                 AppDbContext dbContext)
        {
            _travelPlanRepository = travelPlanRepository;
            _userRepository = userRepository;
            _userTravelPlanService = userTravelPlanService;
            _travelPlanStatusService = travelPlanStatusService;
            _dbContext = dbContext;
        }

        public async Task<bool> AddTravelerAsync(Guid travelPlanId, Guid userToAddId)
        {
            try
            {
                //check if user exists
                var userExists = await _userRepository.DoesUserExistAsync(userToAddId);
                if (!userExists) throw new Exception("Invalid User Id");

                var isSuccessful = await _travelPlanRepository.AddTravelerAsync(travelPlanId, userToAddId);

                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TravelPlanDto> CreateAsync(TravelPlanDto travelPlanDto, Guid userId)
        {
            try
            {
                //map here

                var newTravelPlan = new Domain.Models.TravelPlan
                {
                    Name = travelPlanDto.Name,
                    Description = travelPlanDto.Description,
                    StartDate = travelPlanDto.StartDate,
                    EndDate = travelPlanDto.EndDate,
                    CreatedById = userId,
                    TravelPlanStatusId = (int)TravelPlanStatusEnum.Created,
                    //add jxn, auto adds to UTP table
                    UserTravelPlans = new List<UserTravelPlan>
                    {
                        new UserTravelPlan
                        {
                            UserId = userId
                        }
                    }
                };

                var tp = await _travelPlanRepository.CreateAsync(newTravelPlan);

                return new TravelPlanDto(tp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid travelPlanId, Guid userId)
        {
            try
            {
                var travelPlanToDelete = await _travelPlanRepository.GetAsync(travelPlanId);
                if (travelPlanToDelete == null) return true;
                if (travelPlanToDelete.CreatedById != userId) throw new InsufficientRightsException("Insufficient rights to delete Travel Plan");

                var isSuccessful = await _travelPlanRepository.DeleteAsync(travelPlanToDelete);
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
                var travelPlanToEdit = await _travelPlanRepository.GetAsync(travelPlanDto.Id);
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

        public async Task<TravelPlanDto> GetAsync(Guid travelPlanId, bool includeUTP = false, bool includeStatus = false)
        {
            var travelPlan = await _travelPlanRepository.GetAsync(travelPlanId, includeUTP);

            if (travelPlan == null) throw new Exception("Travel Plan not found");

            var travelPlanDto = new TravelPlanDto(travelPlan);

            if (includeStatus)
            {
                var tpStatus = await _travelPlanStatusService.GetStatusAsync((TravelPlanStatusEnum)travelPlan.TravelPlanStatusId);

                travelPlanDto.TravelPlanStatus = new TravelPlanStatusDto
                {
                    UniqStatus = tpStatus.UniqStatus,
                    Description = tpStatus.Description
                };
            }

            return travelPlanDto;
        }

        public async Task<List<TravelPlanDto>> ListAsync(Guid userId, int? status = null)
        {
            try
            {
                //get travel plans associated with the user, whether they created it or just belong it
                var userTravelPlanIds = await _userTravelPlanService.GetUserTravelPlanIDsAsync(userId);
                var travelPlans = await _travelPlanRepository.GetTravelPlansWithFilterAsync(userTravelPlanIds, status);

                List<TravelPlanDto> lstTravelPlanDto = new List<TravelPlanDto>();

                foreach (var tp in travelPlans)
                {
                    var tpDto = new TravelPlanDto(tp);
                    var tpStatus = await _travelPlanStatusService.GetStatusAsync((TravelPlanStatusEnum)tp.TravelPlanStatusId);

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
                var travelPlan = await _travelPlanRepository.GetAsync(travelPlanId, true);

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

                var userTPToRemove = travelPlan.UserTravelPlans?.Where((utp) => utp.UserId.ToString() == travelerToRemove.Id).FirstOrDefault();

                //if user actually was never part of the utp then just return
                if (userTPToRemove == null)
                {
                    return true;
                }

                
                var isUserHost = loggedInUserId == travelPlan.CreatedById;

                if(isUserHost && loggedInUserId == userTPToRemove.UserId)
                {
                    throw new Exception("Host can't remove themselves from plan");
                }

                var userNotHostButIsTraveler = !isUserHost && loggedInUserId.ToString() == travelerToRemove.Id;


                //hosts have delete rights or the travelers want to remove themselves
                if (isUserHost || userNotHostButIsTraveler)
                {
                    var isSuccessful = await _userTravelPlanService.Delete(userTPToRemove);
                    return isSuccessful;
                }
                else
                {
                    throw new InsufficientRightsException("Insufficient rights to Travel Plan");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Dictionary<string, TravelPlanStatusDto>> SetStatusAsync(Guid travelPlanId, Guid userId, int status)
        {
            try
            {
                if (!Enum.IsDefined(typeof(TravelPlanStatusEnum), status))
                {
                    throw new Exception("Problem Setting Status of Travel Plan");
                }
                var travelPlanToEdit = await _travelPlanRepository.GetAsync(travelPlanId);
                if (travelPlanToEdit == null) throw new Exception("Travel Plan Not Found");

                if (travelPlanToEdit.CreatedById != userId) throw new InsufficientRightsException("Insufficient rights to edit Travel Plan");
                travelPlanToEdit.TravelPlanStatusId = status;

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;
                if (isSuccessful)
                {
                    var tpStatus = await _travelPlanStatusService.GetStatusAsync((TravelPlanStatusEnum)status);
                    return new Dictionary<string, TravelPlanStatusDto> { { travelPlanId.ToString(),
                        new TravelPlanStatusDto
                        {
                            UniqStatus = tpStatus.UniqStatus,
                            Description = tpStatus.Description
                        }
                    } };
                }
                throw new Exception("Problem occurred saving status of Travel Plan");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}