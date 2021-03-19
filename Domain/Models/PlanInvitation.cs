using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PlanInvitation
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Guid InvitedById { get; set; } //who invited
        public Guid InviteeId { get; set; } //who to invite

        //nav props
        public Guid TravelPlanId { get; set; }
        public TravelPlan TravelPlan { get; set; }
        public PlanInvitation()
        {

        }
    }
}
