using Business.TravelPlan.Interfaces;
using DataAccess.Common.Enums;
using DataAccess.CustomExceptions;
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
        private readonly IUserTravelPlanRepository _userTravelPlanRepository;
        private readonly IPlanInvitationRepository _planInvitationRepository;
        private readonly ITravelPlanService _travelPlanService;
        private readonly IUserRepository _userRepository;

        public TravelPlanController(ITravelPlanRepository travelPlanRepository,
                                    IUserRepository userRepository,
                                    IUserTravelPlanRepository userTravelPlanRepository,
                                    IPlanInvitationRepository planInvitationRepository,
                                    ITravelPlanService travelPlanService)
        {
            _userRepository = userRepository;
            _userTravelPlanRepository = userTravelPlanRepository;
            _planInvitationRepository = planInvitationRepository;
            _travelPlanService = travelPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanDto travelPlanDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var newTravelPlan = await _travelPlanService.CreateAsync(travelPlanDto, new Guid(userId));

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
                var editedTravelPlanDto = await _travelPlanService.EditAsync(travelPlanDto, new Guid(userId));

                return Ok(editedTravelPlanDto);
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    Message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetStatus([FromQuery] string id, [FromQuery] int status)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var tpIdAndStatusDto = await _travelPlanService.SetStatusAsync(new Guid(id), new Guid(userId), status);

                return Ok(tpIdAndStatusDto);
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    Message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest(new { Message = "Error occurred updating status" });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var isSuccessful = await _travelPlanService.DeleteAsync(new Guid(id), new Guid(userId));

                if (!isSuccessful) return StatusCode(500);

                return Ok();
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    Message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromQuery] string id)
        {
            try
            {
                var travelPlanId = new Guid(id);
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var travelers = await _userTravelPlanRepository.GetTravelersForActivityAsync(travelPlanId);
                if (!travelers.Contains(new Guid(userId)))
                {
                    return Forbid();
                }

                var userTravelers = await _userRepository.GetUsersAsync(travelers);
                var travelPlanDTO = await _travelPlanService.GetAsync(travelPlanId, includeStatus: true);

                travelPlanDTO.Travelers = userTravelers.ToList();

                return Ok(travelPlanDTO);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int? status = null)
        {
            try
            {

                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var lstTravelPlanDTO = await _travelPlanService.ListAsync(new Guid(loggedInUserId), status);

                return Ok(lstTravelPlanDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTraveler(string travelerUsername, Guid travelPlanId)
        {
            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var isSuccessful = await _travelPlanService.RemoveTraveler(new Guid(loggedInUserId), travelerUsername, travelPlanId);

                if (!isSuccessful)
                {
                    return BadRequest(new
                    {
                        Message = "Could not remove traveler"
                    });
                }

                return Ok();
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    Message = insufRights.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest(new
                {
                    Message = "An error occurred sending invitation"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvitation(string inviteeUsername, Guid travelPlanId)
        {
            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                await _planInvitationRepository.InviteUser(new Guid(loggedInUserId), inviteeUsername, travelPlanId);

                return Ok();
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    Message = insufRights.Message
                });
            }
            catch (UserNotFoundException notFoundExc)
            {
                return BadRequest(new
                {
                    Message = notFoundExc.Message
                });
            }
            catch (UniqueConstraintException uniqExc)
            {
                return BadRequest(new
                {
                    Message = uniqExc.Message
                });
            }
            catch (CommonException commExc)
            {
                return BadRequest(new
                {
                    Message = commExc.Message
                });
            }
            catch (Exception exc)
            {
                return BadRequest(new
                {
                    Message = "An error occurred sending invitation"
                });
            }
        }
    }
}