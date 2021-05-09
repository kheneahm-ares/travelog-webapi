using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TPAnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid TravelPlanId { get; set; }
        public Guid? TravelPlanActivityId { get; set; }
        public TPAnnouncementDto()
        {

        }
        public TPAnnouncementDto(TPAnnouncement tpAnnouncement)
        {
            this.Id = tpAnnouncement.Id;
            this.Title = tpAnnouncement.Title;
            this.Description = tpAnnouncement.Description;
            this.CreatedDate = DateTime.SpecifyKind(tpAnnouncement.CreatedDate, DateTimeKind.Utc);
            this.CreatedById = tpAnnouncement.CreatedById;
            this.TravelPlanId = tpAnnouncement.TravelPlanId;
            this.TravelPlanActivityId = tpAnnouncement.TravelPlanActivityId;
        }
    }
}
