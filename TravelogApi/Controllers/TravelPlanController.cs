using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
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
        private readonly IPlanInvitationRepository _planInvitationRepository;
        private readonly IUserRepository _userRepository;

        public TravelPlanController(ITravelPlanRepository travelPlanRepository, 
                                    IUserRepository userRepository, 
                                    IUserTravelPlanRepository userTravelPlanRepository,
                                    IPlanInvitationRepository planInvitationRepository)
        {
            _travelPlanRepository = travelPlanRepository;
            _userRepository = userRepository;
            _userTravelPlanRepository = userTravelPlanRepository;
            _planInvitationRepository = planInvitationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanDto travelPlanDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var newTravelPlan = await _travelPlanRepository.CreateAsync(travelPlanDto, new Guid(userId));

                return Ok(newTravelPlan);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TravelPlanDto travelPlanDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var editedTravelPlanDto = await _travelPlanRepository.EditAsync(travelPlanDto, new Guid(userId));

                return Ok(editedTravelPlanDto);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var isSuccessful = await _travelPlanRepository.DeleteAsync(new Guid(id), new Guid(userId));

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
            var travelPlanDTO = await _travelPlanRepository.GetAsync(travelPlanId);

            travelPlanDTO.Travelers = userTravelers.ToList();

            return Ok(travelPlanDTO);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var lstTravelPlanDTO = await _travelPlanRepository.ListAsync(new Guid(loggedInUserId));

            return Ok(lstTravelPlanDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvitation(Guid userToInvite, Guid travelPlanId)
        {
            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                await _planInvitationRepository.InviteUser(new Guid(loggedInUserId), userToInvite, travelPlanId);

                return Ok();
            }
            catch
            { 
                return BadRequest();
            }

        }
    }
}