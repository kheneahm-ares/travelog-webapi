using Business.TravelPlan.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = Domain.Models;
using DTOs = Domain.DTOs;
using DataAccess.Repositories.Interfaces;
using Persistence;
using Business.TravelPlan;
using Microsoft.EntityFrameworkCore;
using DataAccess.CustomExceptions;

namespace TravelogApi.Tests.Business.TravelPlan
{
    [TestClass]
    public class TPActivityServiceTests
    {

        private readonly MockRepository _mockRepo = new MockRepository(MockBehavior.Strict);
        private DTOs.TravelPlanActivityDto _genericActivityDTO;
        private Guid _emptyTPID;
        private Guid _emptyUserId;
        private DbContextOptions<AppDbContext> _dbOptions;


        //gets called before each test
        [TestInitialize]
        public void Initialize()
        {
            _emptyTPID = new Guid();
            _emptyUserId = new Guid();
            _genericActivityDTO = new DTOs.TravelPlanActivityDto()
            {
                TravelPlanId = _emptyTPID,
                Name = "Activity 1",
                StartTime = new DateTime(),
                EndTime = new DateTime().AddHours(1),
                Location = new DTOs.LocationDto()
                {
                    Address = "Some Place",
                    Latitude = 123.451,
                    Longitude = 543.210
                },
                Category = "Food"
            };

            _dbOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TravelogApi").Options;
        }


        [TestMethod]
        public async Task CreateAsync_ValidActivity_ReturnsActivity()
        {

            var newActivity = new Models.TravelPlanActivity
            {
                Name = _genericActivityDTO.Name,
                StartTime = _genericActivityDTO.StartTime,
                EndTime = _genericActivityDTO.EndTime,
                Category = _genericActivityDTO.Category,
                Location = new Models.Location
                {
                    Address = _genericActivityDTO.Location.Address,
                    Latitude = _genericActivityDTO.Location.Latitude,
                    Longitude = _genericActivityDTO.Location.Longitude,
                },
                HostId = _emptyUserId,
                TravelPlanId = _genericActivityDTO.TravelPlanId
            };

            var travelPlanDto = new DTOs.TravelPlanDto();

            var tpService = _mockRepo.Create<ITravelPlanService>();
            var tpActivityRepo = _mockRepo.Create<ITravelPlanActivityRepository>();

            //arrange
            tpService.Setup((tps) => tps.GetAsync(_emptyTPID, false, false)).ReturnsAsync(travelPlanDto);
            tpActivityRepo.Setup((tpa) => tpa.CreateAsync(It.IsAny<Models.TravelPlanActivity>())).ReturnsAsync(newActivity);

            //act
            using(var context = new AppDbContext(_dbOptions))
            {
                var tpActivityService = new TPActivityService(context, tpService.Object, tpActivityRepo.Object);
                var result = await tpActivityService.CreateAsync(_genericActivityDTO, _emptyUserId);

                //verify
                Assert.IsNotNull(result);
                Assert.IsTrue(result is DTOs.TravelPlanActivityDto);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreateAsync_InvalidTPId_ThrowsException()
        {

            DTOs.TravelPlanDto travelPlanDto = null;

            var tpService = _mockRepo.Create<ITravelPlanService>();
            var tpActivityRepo = _mockRepo.Create<ITravelPlanActivityRepository>();

            //arrange
            tpService.Setup((tps) => tps.GetAsync(_emptyTPID, false, false)).ReturnsAsync(travelPlanDto);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpActivityService = new TPActivityService(context, tpService.Object, tpActivityRepo.Object);
                var result = await tpActivityService.CreateAsync(_genericActivityDTO, _emptyUserId);

                //verify
            }

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task EditAsync_InvalidActivityID_ThrowsException()
        {
            var tpService = _mockRepo.Create<ITravelPlanService>();
            var tpActivityRepo = _mockRepo.Create<ITravelPlanActivityRepository>();
            Models.TravelPlanActivity nullTPActivity = null;

            //arrange
            tpActivityRepo.Setup((tpa) => tpa.GetAsync(It.IsAny<Guid>())).ReturnsAsync(nullTPActivity);
            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpActivityService = new TPActivityService(context, tpService.Object, tpActivityRepo.Object);
                var result = await tpActivityService.EditAsync(_genericActivityDTO, _emptyUserId);

                //verify
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRightsException))]
        public async Task EditAsync_UserNotHost_ThrowsInsufficientRightsException()
        {
            var tpService = _mockRepo.Create<ITravelPlanService>();
            var tpActivityRepo = _mockRepo.Create<ITravelPlanActivityRepository>();
            var tpActivityToEdit = new Models.TravelPlanActivity
            {
                HostId = Guid.NewGuid()
            };

            //arrange
            tpActivityRepo.Setup((tpa) => tpa.GetAsync(It.IsAny<Guid>())).ReturnsAsync(tpActivityToEdit);
            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpActivityService = new TPActivityService(context, tpService.Object, tpActivityRepo.Object);
                var result = await tpActivityService.EditAsync(_genericActivityDTO, _emptyUserId);

                //verify
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRightsException))]
        public async Task DeleteAsync_UserNotHost_ThrowsInsufficientRightsException()
        {
            var tpService = _mockRepo.Create<ITravelPlanService>();
            var tpActivityRepo = _mockRepo.Create<ITravelPlanActivityRepository>();
            var tpActivityToDelete = new Models.TravelPlanActivity
            {
                HostId = Guid.NewGuid()
            };

            //arrange
            tpActivityRepo.Setup((tpa) => tpa.GetAsync(It.IsAny<Guid>())).ReturnsAsync(tpActivityToDelete);
            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpActivityService = new TPActivityService(context, tpService.Object, tpActivityRepo.Object);
                var result = await tpActivityService.EditAsync(_genericActivityDTO, _emptyUserId);

                //verify
            }
        }



    }
}
