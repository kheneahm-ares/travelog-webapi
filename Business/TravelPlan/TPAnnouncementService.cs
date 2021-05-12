using Business.TravelPlan.Interfaces;
using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Persistence;
using System;
using System.Threading.Tasks;

namespace Business.TravelPlan
{
    public class TPAnnouncementService : ITPAnnouncementService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITravelPlanService _travelPlanService;
        private readonly ITPAnnouncementRepository _announcementRepository;

        public TPAnnouncementService(AppDbContext dbContext, ITravelPlanService travelPlanService, ITPAnnouncementRepository announcementRepository)
        {
            _dbContext = dbContext;
            _travelPlanService = travelPlanService;
            _announcementRepository = announcementRepository;
        }

        public async Task<TPAnnouncementDto> CreateAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId)
        {
            try
            {
                var tpToAnnounceTo = await _travelPlanService.GetAsync(announcementDto.TravelPlanId);

                if (tpToAnnounceTo.CreatedById != loggedInUserId)
                {
                    throw new InsufficientRightsException("User doesn't have rights to delete");
                }

                var newAnnouncement = new TPAnnouncement
                {
                    Title = announcementDto.Title,
                    Description = announcementDto.Description,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = loggedInUserId,
                    TravelPlanId = announcementDto.TravelPlanId,
                    TravelPlanActivityId = announcementDto.TravelPlanActivityId
                };

                var addedAnnouncement = await _announcementRepository.CreateAsync(newAnnouncement);

                return new TPAnnouncementDto(addedAnnouncement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid announcementId, Guid loggedInUserId)
        {
            try
            {
                //get list of announcements for travel plan
                var announcementToDelete = await _announcementRepository.GetAsync(announcementId);

                if (announcementToDelete == null)
                {
                    return true;
                }

                if (announcementToDelete.CreatedById != loggedInUserId)
                {
                    throw new InsufficientRightsException("User doesn't have rights to delete");
                }

                var isSuccessful = await _announcementRepository.DeleteAsync(announcementToDelete);
                return isSuccessful;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TPAnnouncementDto> EditAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId)
        {
            try
            {
                //validate announcement exists
                var announcementToEdit = await _announcementRepository.GetAsync(announcementDto.Id);

                if (announcementToEdit == null)
                {
                    throw new CommonException("Invalid Announcement");
                }

                //validate logged in user is even able to make an announcement to travel plan
                var tpToAnnounceTo = await _travelPlanService.GetAsync(announcementDto.TravelPlanId);

                if (tpToAnnounceTo.CreatedById != loggedInUserId)
                {
                    throw new InsufficientRightsException("User doesn't have rights to delete");
                }

                announcementToEdit.Title = announcementDto.Title;
                announcementToEdit.Description = announcementDto.Description;
                announcementToEdit.CreatedDate = announcementDto.CreatedDate; //probably create a new column for updated date

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return new TPAnnouncementDto(announcementToEdit);
                }
                throw new CommonException("Problem occurred creating announcement");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TPAnnouncementDto> GetAsync(Guid announcementId)
        {
            try
            {
                var announcement = await _announcementRepository.GetAsync(announcementId);

                if (announcement == null)
                {
                    throw new CommonException("Invalid Announcement");
                }

                return new TPAnnouncementDto(announcement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AnnouncementEnvelope> ListAsync(Guid travelPlanId, int limit, int offset)
        {
            try
            {
                var tPlan = await _travelPlanService.GetAsync(travelPlanId);

                if (tPlan == null)
                {
                    throw new CommonException("Invalid TravelPlan");
                }

                var annEnvelope = await _announcementRepository.ListAsync(travelPlanId, limit, offset);

                return annEnvelope;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}