using Business.TravelPlan.Interfaces;
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
    public class TravelPlanActivityController : Controller
    {
        private readonly ITPActivityService _activityService;
        private readonly IUserTravelPlanService _userTravelPlanService;

        public TravelPlanActivityController(ITPActivityService activityService, IUserTravelPlanService userTravelPlanService)
        {
            _activityService = activityService;
            _userTravelPlanService = userTravelPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanActivityDto activityDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var newActivity = await _activityService.CreateAsync(activityDto, new Guid(userId));

                return Ok(newActivity);
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TravelPlanActivityDto activityDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var editedActivityDto = await _activityService.EditAsync(activityDto, new Guid(userId));

                return Ok(editedActivityDto);
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    message = insufRights.Message
                });
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
                var isSuccessful = await _activityService.DeleteAsync(new Guid(id), new Guid(userId));

                if (!isSuccessful) return StatusCode(500);

                return Ok();
            }
            catch (InsufficientRightsException insufRights)
            {
                return BadRequest(new
                {
                    message = insufRights.Message
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
            var activityDto = await _activityService.GetAsync(new Guid(id));

            return Ok(activityDto);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string id)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var travelPlanId = new Guid(id);
                var travelers = await _userTravelPlanService.GetTravelersForActivityAsync(travelPlanId);
                if(travelers.Count() == 0)
                {
                    //travel plan doesn't exist
                    return BadRequest(new
                    {
                        Message = "Error occurred retrieving activities for travel plan"
                    });
                }
                if (!travelers.Contains(new Guid(userId)))
                {
                    return Forbid();
                }

                var lstActivityDto = await _activityService.ListAsync(travelPlanId);

                return Ok(lstActivityDto);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}