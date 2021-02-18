using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(AppDbContext context)
        {
            if (!context.TravelPlans.Any())
            {
                //create travelplans
                var travelPlans = new List<TravelPlan>
                {
                    new TravelPlan
                    {
                        Name = "Travel to Europe",
                        StartDate = DateTime.Now.AddMonths(2),
                        EndDate = DateTime.Now.AddMonths(2).AddDays(7),
                        Description = "Traveling to Europe with friends 2021",
                        CreatedById = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54"),
                        TravelPlanActivities = new List<TravelPlanActivity>
                        {
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Portugal",
                                StartTime = DateTime.Now.AddMonths(2).AddDays(1).AddHours(5),
                                EndTime = DateTime.Now.AddMonths(2).AddDays(1).AddHours(6),
                                Category = "Food",
                                Location = "Lisbon, Portugal",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Portugal",
                                StartTime = DateTime.Now.AddMonths(2).AddDays(1).AddHours(10),
                                EndTime = DateTime.Now.AddMonths(2).AddDays(1).AddHours(12),
                                Category = "Fun",
                                Location = "Lisbon, Portugal",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Spain",
                                StartTime = DateTime.Now.AddMonths(2).AddDays(2).AddHours(5),
                                EndTime = DateTime.Now.AddMonths(2).AddDays(2).AddHours(6),
                                Category = "Food",
                                Location = "Madrid, Spain",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Spain",
                                StartTime = DateTime.Now.AddMonths(2).AddDays(2).AddHours(10),
                                EndTime = DateTime.Now.AddMonths(2).AddDays(2).AddHours(12),
                                Category = "Fun",
                                Location = "Madrid, Spain",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            }
                        },
                        UserTravelPlans = new List<UserTravelPlan>
                        {
                            new UserTravelPlan
                            {
                                UserId = new Guid("b828dea1-1fa0-433d-8a8a-26d496f94792")
                            },
                            new UserTravelPlan
                            {
                                UserId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            }
                        }
                    },
                    new TravelPlan
                    {
                        Name = "Travel to Texas",
                        StartDate = DateTime.Now.AddMonths(3),
                        EndDate = DateTime.Now.AddMonths(3).AddDays(7),
                        Description = "Traveling to Europe with friends 2021",
                        CreatedById = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54"),
                        TravelPlanActivities = new List<TravelPlanActivity>
                        {
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Austin",
                                StartTime = DateTime.Now.AddMonths(3).AddDays(1).AddHours(5),
                                EndTime = DateTime.Now.AddMonths(3).AddDays(1).AddHours(6),
                                Category = "Food",
                                Location = "Austin, Texas",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Mall in Austin",
                                StartTime = DateTime.Now.AddMonths(3).AddDays(1).AddHours(10),
                                EndTime = DateTime.Now.AddMonths(3).AddDays(1).AddHours(12),
                                Category = "Fun",
                                Location = "Austin, Texas",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Dallas",
                                StartTime = DateTime.Now.AddMonths(3).AddDays(2).AddHours(5),
                                EndTime = DateTime.Now.AddMonths(3).AddDays(2).AddHours(6),
                                Category = "Food",
                                Location = "Dallas, Texas",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Museum in Dallas",
                                StartTime = DateTime.Now.AddMonths(3).AddDays(2).AddHours(10),
                                EndTime = DateTime.Now.AddMonths(3).AddDays(2).AddHours(12),
                                Category = "Fun",
                                Location = "Dallas, Texas",
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            }
                        },
                        UserTravelPlans = new List<UserTravelPlan>
                        {
                            new UserTravelPlan
                            {
                                UserId = new Guid("b828dea1-1fa0-433d-8a8a-26d496f94792")
                            },
                            new UserTravelPlan
                            {
                                UserId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            }
                        }
                    }
                };

                await context.TravelPlans.AddRangeAsync(travelPlans);
                await context.SaveChangesAsync();
            }
        }
    }
}