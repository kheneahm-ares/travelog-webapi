using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
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