using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// An object containing a user belonging to a specific travelplan
    /// A user can have many travelplans and a travelplan can have many users
    /// This is a jxn table for a the many-to-many relationship
    /// </summary>
    public class UserTravelPlan
    {
        public Guid TravelPlanId { get; set; }
        public TravelPlan TravelPlan { get; set; }
        public Guid ApiUserId { get; set; }
        public ApiUser ApiUser { get; set; }
    }
}
