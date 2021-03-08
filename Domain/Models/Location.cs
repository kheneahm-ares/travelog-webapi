﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        //nav props
        public Guid TravelPlanActivityId { get; set; }
        public virtual TravelPlanActivity TravelPlanActivity { get; set; }
    }
}
