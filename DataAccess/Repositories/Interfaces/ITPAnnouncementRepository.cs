using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITPAnnouncementRepository
    {
        Task<AnnouncementEnvelope> ListAsync(Guid travelPlanId, int limit, int offset);
        Task<TPAnnouncementDto> GetAsync(Guid announcementId);
        Task<bool> DeleteAsync(Guid announcementId, Guid loggedInUserId);
        Task<TPAnnouncementDto> CreateAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId);
        Task<TPAnnouncementDto> EditAsync(TPAnnouncementDto announcementDto, Guid loggedInUserId);
    }
}
