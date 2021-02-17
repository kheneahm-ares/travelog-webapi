using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TravelPlanActivityDto
    {
        public Guid Id { get; set; }
        public Guid TravelPlanId { get; set; }
        public Guid HostId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }

        public TravelPlanActivityDto()
        {

        }

        public TravelPlanActivityDto(TravelPlanActivity travelPlanActivity)
        {
            this.Id = travelPlanActivity.TravelPlanActivityId;
            this.TravelPlanId = travelPlanActivity.TravelPlanId;
            this.HostId = travelPlanActivity.HostId;
            this.Name = travelPlanActivity.Name;
            this.StartTime = travelPlanActivity.StartTime;
            this.EndTime = travelPlanActivity.EndTime;
            this.Category = travelPlanActivity.Category;
            this.Location = travelPlanActivity.Location;
        }

    }
}
