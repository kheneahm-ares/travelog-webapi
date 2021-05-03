using DataAccess.Common.Enums;
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
            if(!context.TravelPlanStatuses.Any())
            {
                var travelPlanStatuses = new List<TravelPlanStatus>
                {
                    new TravelPlanStatus
                    {
                        UniqStatus =  TravelPlanStatusEnum.Created,
                        Description = TravelPlanStatusEnum.Created.ToString()
                    },
                    new TravelPlanStatus
                    {
                        UniqStatus = TravelPlanStatusEnum.OnGoing,
                        Description = TravelPlanStatusEnum.OnGoing.ToString()
                    },
                    new TravelPlanStatus
                    {
                        UniqStatus =  TravelPlanStatusEnum.Completed,
                        Description = TravelPlanStatusEnum.Completed.ToString()
                    },
                    new TravelPlanStatus
                    {
                        UniqStatus = TravelPlanStatusEnum.Archived,
                        Description = TravelPlanStatusEnum.Archived.ToString()
                    }
                };
                await context.TravelPlanStatuses.AddRangeAsync(travelPlanStatuses);
                await context.SaveChangesAsync();
            }

            if (!context.TravelPlans.Any())
            {
                //create travelplans
                var travelPlans = new List<TravelPlan>
                {
                    new TravelPlan
                    {
                        Name = "Travel to Europe",
                        StartDate = DateTime.UtcNow.AddMonths(2),
                        EndDate = DateTime.UtcNow.AddMonths(2).AddDays(7),
                        Description = "Traveling to Europe with friends 2021",
                        CreatedById = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54"),
                        TravelPlanStatusId = (int) TravelPlanStatusEnum.Created,
                        TravelPlanActivities = new List<TravelPlanActivity>
                        {
                            new TravelPlanActivity
                            {
                                Name = "Going to Breakfast in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(1).AddHours(5),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(1).AddHours(6),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(2).AddHours(6),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(2).AddHours(7),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Museum in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(3).AddHours(7),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(3).AddHours(8),
                                Category = "Sightseeing",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Bar in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(4).AddHours(8),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(4).AddHours(9),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Club in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(5).AddHours(9),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(5).AddHours(10),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Portugal",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(6).AddHours(10),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(6).AddHours(12),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Lisbon, Portugal",
                                    Latitude = 38.7223,
                                    Longitude = -9.1427},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Breakfast in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(7).AddHours(5),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(7).AddHours(6),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(8).AddHours(6),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(8).AddHours(7),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Museum in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(9).AddHours(7),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(9).AddHours(8),
                                Category = "Sightseeing",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Bar in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(10).AddHours(8),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(10).AddHours(9),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Club in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(11).AddHours(9),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(11).AddHours(10),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Spain",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(12).AddHours(10),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(12).AddHours(12),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Madrid, Spain",
                                    Latitude = 40.4168,
                                    Longitude = -3.7038},
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
                        StartDate = DateTime.UtcNow.AddMonths(3),
                        EndDate = DateTime.UtcNow.AddMonths(3).AddDays(7),
                        Description = "Traveling to Europe with friends 2021",
                        CreatedById = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54"),
                        TravelPlanStatusId = (int) TravelPlanStatusEnum.Created,
                        TravelPlanActivities = new List<TravelPlanActivity>
                        {
                            new TravelPlanActivity
                            {
                                Name = "Going to Breakfast in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(1).AddHours(5),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(1).AddHours(6),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(2).AddHours(6),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(2).AddHours(7),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Museum in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(3).AddHours(7),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(3).AddHours(8),
                                Category = "Sightseeing",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Bar in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(4).AddHours(8),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(4).AddHours(9),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Club in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(5).AddHours(9),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(5).AddHours(10),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Austin",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(6).AddHours(10),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(6).AddHours(12),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Austin, Texas",
                                    Latitude = 30.2666,
                                    Longitude = -97.7333},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Breakfast in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(7).AddHours(5),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(7).AddHours(6),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Cafe in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(8).AddHours(6),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(8).AddHours(7),
                                Category = "Food",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Museum in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(9).AddHours(7),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(9).AddHours(8),
                                Category = "Sightseeing",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Bar in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(10).AddHours(8),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(10).AddHours(9),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Club in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(11).AddHours(9),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(11).AddHours(10),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
                                HostId = new Guid("1a7dbb84-6de2-4bfc-97e0-31ad64c3ed54")
                            },
                            new TravelPlanActivity
                            {
                                Name = "Going to Beach in Dallas",
                                StartTime = DateTime.UtcNow.AddMonths(2).AddDays(12).AddHours(10),
                                EndTime = DateTime.UtcNow.AddMonths(2).AddDays(12).AddHours(12),
                                Category = "Fun",
                                Location = new Location() {
                                    Address = "Dallas, Texas",
                                    Latitude = 32.7767,
                                    Longitude = -96.7970},
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