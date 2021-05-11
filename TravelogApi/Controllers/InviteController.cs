using Business.TravelPlan.Interfaces;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class InviteController : Controller
    {
        private readonly ITravelPlanInvitationService _travelPlanInvitationService;

        public InviteController(ITravelPlanInvitationService travelPlanInvitationService)
        {
            _travelPlanInvitationService = travelPlanInvitationService;
        }

        public async Task<IActionResult> List()
        {

            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var userInvitations = await _travelPlanInvitationService.List(new Guid(loggedInUserId));

                return Ok(userInvitations);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Accept(int inviteId)
        {
            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                await _travelPlanInvitationService.AcceptInvitation(new Guid(loggedInUserId), inviteId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int inviteId)
        {
            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                await _travelPlanInvitationService.DeclineInvitation(new Guid(loggedInUserId), inviteId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}