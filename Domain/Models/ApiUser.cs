using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Will be used by IdentityServer to create a user once a user is registered in Identity's db
    /// </summary>
    public class ApiUser
    {
        public Guid ApiUserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }


        //jxn table
        public virtual ICollection<UserTravelPlan> UserTravelPlans { get; set; }
    }
}
