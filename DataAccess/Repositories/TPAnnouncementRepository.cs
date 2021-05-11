using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TPAnnouncementRepository : ITPAnnouncementRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ITravelPlanRepository _travelPlanRepository;

        public TPAnnouncementRepository(AppDbContext dbContext, ITravelPlanRepository travelPlanRepository)
        {
            _dbContext = dbContext;
            _travelPlanRepository = travelPlanRepository;
        }

        public async Task<TPAnnouncementDto> CreateAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId)
        {
            try
            {
                //validate logged in user is even able to make an announcement to travel plan
                var tpToAnnounceTo = await _travelPlanRepository.GetAsync(announcementDto.TravelPlanId);

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

                _dbContext.TPAnnouncements.Add(newAnnouncement);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return announcementDto;
                }
                throw new CommonException("Problem occurred creating announcement");
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
                var announcementToDelete = await _dbContext.TPAnnouncements.FindAsync(announcementId);

                if (announcementToDelete == null)
                {
                    return true;
                }

                if (announcementToDelete.CreatedById != loggedInUserId)
                {
                    throw new InsufficientRightsException("User doesn't have rights to delete");
                }

                _dbContext.TPAnnouncements.Remove(announcementToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<TPAnnouncementDto> EditAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId)
        {
            try
            {
                //validate announcement exists
                var announcementToEdit = await _dbContext.TPAnnouncements.FindAsync(announcementDto.Id);

                if (announcementToEdit == null)
                {
                    throw new CommonException("Invalid Announcement");
                }

                //validate logged in user is even able to make an announcement to travel plan
                var tpToAnnounceTo = await _travelPlanRepository.GetAsync(announcementDto.TravelPlanId);

                if (tpToAnnounceTo.CreatedById != loggedInUserId)
                {
                    throw new InsufficientRightsException("User doesn't have rights to delete");
                }

                announcementToEdit.Title = announcementDto.Title;
                announcementToEdit.Description = announcementDto.Description;
                announcementToEdit.CreatedDate = announcementDto.CreatedDate; //probably create a new column for updated date

                if (!_dbContext.ChangeTracker.HasChanges()) return announcementDto;

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
                //get list of announcements for travel plan
                var announcement = await _dbContext.TPAnnouncements.FindAsync(announcementId);

                if (announcement == null)
                {
                    throw new CommonException("Invalid Announcement");
                }

                return new TPAnnouncementDto(announcement);
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<AnnouncementEnvelope> ListAsync(Guid travelPlanId, int limit, int offset)
        {
            try
            {
                //get list of announcements for travel plan
                var tPlan = await _dbContext.TravelPlans.FindAsync(travelPlanId);

                if (tPlan == null)
                {
                    throw new CommonException("Invalid TravelPlan");
                }

                var annQueryable = _dbContext.TPAnnouncements
                                    .Where((tpa) => tpa.TravelPlanId == travelPlanId)
                                    .OrderByDescending((tpa) => tpa.CreatedDate)
                                    .AsQueryable();

                var announcements = await annQueryable
                                            .Skip(offset)
                                            .Take(limit)
                                            .ToListAsync();

                var announcementDTOs = announcements.Select((a) => new TPAnnouncementDto(a)).ToList();

                return new AnnouncementEnvelope
                {
                    AnnouncementDtos = announcementDTOs,
                    AnnouncementCount = annQueryable.Count()
                };
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}