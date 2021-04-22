using Domain.Models;
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

        public string TravelPlanName { get; set; }
        public string InviterUsername { get; set; }

        public Guid InviteeId { get; set; } //who to invite
        public Guid InvitedById { get; set; } //who invited
        public Guid TravelPlanId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public PlanInvitationDto()
        {

        }
    }
}
