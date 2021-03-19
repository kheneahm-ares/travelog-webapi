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
        public Guid InvitedById { get; set; } //who invited
        public Guid InviteeId { get; set; } //who to invite
        public Guid TravelPlanId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }

        public PlanInvitation()
        {

        }
    }
}
