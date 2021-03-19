using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class PlanInvitationDto
    {
        public int Id { get; set; }
        public Guid InvitedById { get; set; } //who invited
        public Guid InviteeId { get; set; } //who to invite
        public Guid TravelPlanId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PlanInvitationDto()
        {

        }
    }
}
