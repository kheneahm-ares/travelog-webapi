using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TravelPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }
        public List<UserDto> Travelers { get; set; }

        public TravelPlanDto()
        {

        }
        public TravelPlanDto(TravelPlan travelPlan)
        {
            this.Id = travelPlan.TravelPlanId;
            this.Name = travelPlan.Name;
            this.StartDate = travelPlan.StartDate;
            this.EndDate = travelPlan.EndDate;
            this.Description = travelPlan.Description;
            this.CreatedById = travelPlan.CreatedById;
        }

    }
}
