using DataAccess.Common.Enums;

namespace Domain.Models
{
    public class TravelPlanStatus
    {
        public int TravelPlanStatusId { get; set; }
        public TravelPlanStatusEnum UniqStatus { get; set; }
        public string Description { get; set; }
    }
}