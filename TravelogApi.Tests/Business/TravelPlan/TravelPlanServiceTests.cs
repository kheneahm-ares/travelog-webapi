using Business.TravelPlan;
using Business.TravelPlan.Interfaces;
using DataAccess.Common.Enums;
using DataAccess.CustomExceptions;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Persistence;
using System;
using System.Threading.Tasks;
using Models = Domain.Models;
using DTOs = Domain.DTOs;
using System.Collections.Generic;

namespace TravelogApi.Tests.Business.TravelPlan
{
    [TestClass]
    public class TravelPlanServiceTests
    {
        private readonly MockRepository _mockRepo = new MockRepository(MockBehavior.Strict);
        private DbContextOptions<AppDbContext> _dbOptions;
        private Guid _emptyTPId;
        private Guid _emptyUserId;

        [TestInitialize]
        public void Initialize()
        {
            _emptyTPId = new Guid();
            _emptyUserId = new Guid();
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TravelogApi").Options;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task SetStatus_InvalidStatus_ThrowsException()
        {
            var loggedInUserId = _emptyUserId;

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            int invalidStatus = 123;

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.SetStatusAsync(loggedInUserId, _emptyUserId, invalidStatus);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task SetStatus_InvalidTravelPlan_ThrowsException()
        {
            var loggedInUserId = _emptyUserId;

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            int invalidStatus = (int)TravelPlanStatusEnum.Archived;

            var tpToEdit = new Models.TravelPlan();

            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(It.IsAny<Guid>(), false)).ReturnsAsync(tpToEdit);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.SetStatusAsync(loggedInUserId, _emptyUserId, invalidStatus);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRightsException))]
        public async Task SetStatus_UserIsNotCreator_ThrowsInsufficientRightsException()
        {
            var loggedInUserId = _emptyUserId;

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            int status = (int)TravelPlanStatusEnum.Archived;

            var tpToEdit = new Models.TravelPlan() { CreatedById = Guid.NewGuid() };

            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(It.IsAny<Guid>(), false)).ReturnsAsync(tpToEdit);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.SetStatusAsync(loggedInUserId, _emptyUserId, status);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RemoveTraveler_InvalidTravelPlan_ThrowsException()
        {
            var loggedInUserId = _emptyUserId;

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            Models.TravelPlan nullTP = null;

            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(_emptyTPId, true)).ReturnsAsync(nullTP);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.RemoveTraveler(loggedInUserId, "someUsername", _emptyTPId);
            }

        }

        [TestMethod]
        public async Task RemoveTraveler_InvalidTraveler_ReturnsTrue()
        {
            var loggedInUserId = _emptyUserId;
            var userName = "someUsername";

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            Models.TravelPlan travelPlan = new Models.TravelPlan();
            DTOs.UserDto nullUser = null;


            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(_emptyTPId, true)).ReturnsAsync(travelPlan);
            userRepo.Setup((ur) => ur.GetUserAsync(userName)).ReturnsAsync(nullUser);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.RemoveTraveler(loggedInUserId, userName, _emptyTPId);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task RemoveTraveler_UserTravelPlanNotExists_ReturnsTrue()
        {
            var loggedInUserId = _emptyUserId;
            var userName = "someUsername";

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            Models.TravelPlan travelPlan = new Models.TravelPlan();
            travelPlan.UserTravelPlans = null;
            DTOs.UserDto user = new DTOs.UserDto();


            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(_emptyTPId, true)).ReturnsAsync(travelPlan);
            userRepo.Setup((ur) => ur.GetUserAsync(userName)).ReturnsAsync(user);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.RemoveTraveler(loggedInUserId, userName, _emptyTPId);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RemoveTraveler_HostRemoveThemslves_ThrowsException()
        {
            var loggedInUserId = _emptyUserId;
            var userName = "someUsername";

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            Models.TravelPlan travelPlan = new Models.TravelPlan();
            travelPlan.CreatedById = _emptyUserId;


            //user to remove is the host 
            var userTPToRemove = new Models.UserTravelPlan
            {
                UserId = _emptyUserId
            };


            travelPlan.UserTravelPlans = new List<Models.UserTravelPlan>
            {
                userTPToRemove    
            };

            DTOs.UserDto user = new DTOs.UserDto();
            user.Id = _emptyUserId.ToString();


            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(_emptyTPId, true)).ReturnsAsync(travelPlan);
            userRepo.Setup((ur) => ur.GetUserAsync(userName)).ReturnsAsync(user);
            userTPService.Setup(utp => utp.Delete(It.IsAny<Models.UserTravelPlan>())).ReturnsAsync(true);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.RemoveTraveler(loggedInUserId, userName, _emptyTPId);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRightsException))]
        public async Task RemoveTraveler_UserNotHostRemoveOtherTraveler_ThrowsInsufficientRIghtsException()
        {
            var loggedInUserId = _emptyUserId;
            var userName = "someUsername";

            var tpRepo = _mockRepo.Create<ITravelPlanRepository>();
            var userRepo = _mockRepo.Create<IUserRepository>();
            var userTPService = _mockRepo.Create<IUserTravelPlanService>();
            var tpStatusService = _mockRepo.Create<ITravelPlanStatusService>();

            Models.TravelPlan travelPlan = new Models.TravelPlan();

            //user is not the host and user to remove is not themselves
            travelPlan.CreatedById = Guid.NewGuid();

            var userTPToRemove = new Models.UserTravelPlan
            {
                UserId = Guid.NewGuid()
            };


            travelPlan.UserTravelPlans = new List<Models.UserTravelPlan>
            {
                userTPToRemove
            };

            DTOs.UserDto user = new DTOs.UserDto();
            user.Id = userTPToRemove.UserId.ToString();

            //arrange
            tpRepo.Setup((tpr) => tpr.GetAsync(_emptyTPId, true)).ReturnsAsync(travelPlan);
            userRepo.Setup((ur) => ur.GetUserAsync(userName)).ReturnsAsync(user);
            userTPService.Setup(utp => utp.Delete(It.IsAny<Models.UserTravelPlan>())).ReturnsAsync(true);

            //act
            using (var context = new AppDbContext(_dbOptions))
            {
                var tpService = new TravelPlanService(tpRepo.Object, userRepo.Object, userTPService.Object, tpStatusService.Object, context);

                var result = await tpService.RemoveTraveler(loggedInUserId, userName, _emptyTPId);
            }
        }
    }
}