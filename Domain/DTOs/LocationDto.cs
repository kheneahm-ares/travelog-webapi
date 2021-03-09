using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LocationDto
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public LocationDto()
        {

        }
        public LocationDto(Location location)
        {
            this.Address = location.Address;
            this.Latitude = location.Latitude;
            this.Longitude = location.Longitude;
        }
    }
}
