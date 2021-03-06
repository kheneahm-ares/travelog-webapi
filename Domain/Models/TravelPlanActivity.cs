﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{

    public class TravelPlanActivity
    {
        public Guid TravelPlanActivityId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Category { get; set; }
        public Guid HostId { get; set; }
        public Guid TravelPlanId { get; set; }

        //nav props
        public TravelPlan TravelPlan{ get; set; }
        public virtual Location Location { get; set; }

    }
}
