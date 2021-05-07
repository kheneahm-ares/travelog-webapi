using System;

namespace Domain.Models
{
    public class TPAnnouncement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid TravelPlanId { get; set; }
        public Guid? TravelPlanActivityId { get; set; }
    }
}