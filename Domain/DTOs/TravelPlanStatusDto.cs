using DataAccess.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TravelPlanStatusDto
    {
        public TravelPlanStatusEnum UniqStatus { get; set; }
        public string Description { get; set; }
    }
}
