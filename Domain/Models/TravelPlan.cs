using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Prinicipal Entity for TravelPlanTraveler and TravelPlanActivity
    /// </summary>
    public class TravelPlan
    {
        public Guid TravelPlanId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }

        //fks
        public int TravelPlanStatusId { get; set; }

        //collection navigation properties
        public List<TravelPlanActivity> TravelPlanActivities { get; set; } 
        public virtual List<UserTravelPlan> UserTravelPlans { get; set; }

    }
}
