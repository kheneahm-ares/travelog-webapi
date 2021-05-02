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
        private readonly IPlanInvitationRepository _planInvitationRepository;

        public InviteController(IPlanInvitationRepository planInvitationRepository)
        {
            _planInvitationRepository = planInvitationRepository;
        }

        public async Task<IActionResult> List()
        {

            try
            {
                var loggedInUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var userInvitations = await _planInvitationRepository.List(new Guid(loggedInUserId));

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

                await _planInvitationRepository.AcceptInvitation(new Guid(loggedInUserId), inviteId);

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

                await _planInvitationRepository.DeclineInvitation(new Guid(loggedInUserId), inviteId);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}