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

        public TPAnnouncementRepository(AppDbContext dbContext, ITravelPlanRepository travelPlanRepository)
        {
            _dbContext = dbContext;
        }

        public async Task<TPAnnouncement> CreateAsync(TPAnnouncement newAnnouncement)
        {
            try
            {

                _dbContext.TPAnnouncements.Add(newAnnouncement);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                if (isSuccessful)
                {
                    return newAnnouncement;
                }
                throw new CommonException("Problem occurred creating announcement");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TPAnnouncement announcementToDelete)
        {
            try
            {
                _dbContext.TPAnnouncements.Remove(announcementToDelete);

                var isSuccessful = await _dbContext.SaveChangesAsync() > 0;

                return isSuccessful;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public async Task<TPAnnouncement> GetAsync(Guid announcementId)
        {
            try
            {
                //get list of announcements for travel plan
                var announcement = await _dbContext.TPAnnouncements.FindAsync(announcementId);

                if (announcement == null)
                {
                    throw new CommonException("Invalid Announcement");
                }

                return announcement;
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