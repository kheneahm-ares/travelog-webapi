using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TravelPlan> TravelPlans { get; set; }
        public DbSet<TravelPlanActivity> TravelPlanActivities { get; set; }
        public DbSet<UserTravelPlan> UserTravelPlans { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PlanInvitation> PlanInvitations { get; set; }
        public DbSet<TravelPlanStatus> TravelPlanStatuses { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {

            //create composite key and use as primary key
            builder.Entity<UserTravelPlan>(config => config.HasKey(ua => new { ua.UserId, ua.TravelPlanId }));
            builder.Entity<PlanInvitation>(config => config.HasAlternateKey(inv => new {inv.InvitedById, inv.InviteeId, inv.TravelPlanId }));

        }

    }
}
