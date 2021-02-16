using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    //[Authorize]
    public class TravelPlanController : Controller
    {
        private readonly ITravelPlanRepository _travelPlanRepository;
        private readonly IUserTravelPlanRepository _userTravelPlanRepository;
        private readonly IUserRepository _userRepository;

        public TravelPlanController(ITravelPlanRepository travelPlanRepository, IUserRepository userRepository, IUserTravelPlanRepository userTravelPlanRepository)
        {
            _travelPlanRepository = travelPlanRepository;
            _userRepository = userRepository;
            _userTravelPlanRepository = userTravelPlanRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanDto travelPlanDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var isSuccessful = await _travelPlanRepository.CreateAsync(travelPlanDto, userId);

                if (!isSuccessful) return StatusCode(500);

                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddTraveler([FromQuery] string travelPlanId, [FromQuery] string userId)
        {
            try
            {
                var isSuccessful = await _travelPlanRepository.AddTravelerAsync(new Guid(travelPlanId), new Guid(userId));

                if (!isSuccessful) return StatusCode(500);

                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromQuery] string id)
        {
            var travelPlanId = new Guid(id);

            var travelers = await _userTravelPlanRepository.GetTravelersForActivityAsync(travelPlanId);
            var userTravelers = await _userRepository.GetUsersAsync(travelers);
            var travelPlan = await _travelPlanRepository.GetAsync(travelPlanId);

            var travelPlanDTO = new TravelPlanDto(travelPlan);
            travelPlanDTO.Travelers = userTravelers.ToList();

            return Ok(travelPlanDTO);

        }
    }
}