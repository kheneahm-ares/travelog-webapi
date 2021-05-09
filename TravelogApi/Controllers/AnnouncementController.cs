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
    [Route("/TravelPlan/[controller]/[action]")]
    public class AnnouncementController : Controller
    {
        private readonly ITPAnnouncementRepository _announcementRepo;

        public AnnouncementController(ITPAnnouncementRepository announcementRepo)
        {
            _announcementRepo = announcementRepo;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] Guid travelPlanId)
        {
            try
            {
                var announcements = await _announcementRepo.ListAsync(travelPlanId);
                return Ok(announcements);
            }
            catch (Exception)
            {

                return BadRequest(new { Message = "Error Occurred" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid announcementId)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var isSuccessful = await _announcementRepo.DeleteAsync(announcementId, new Guid(userId));

                if (isSuccessful)
                {
                    return Ok();
                }
                return BadRequest(new { Message = "Error Occurred" });
            }
            catch (InsufficientRightsException insufExc)
            {
                return BadRequest(new { Message = insufExc.Message });
            }
            catch (CommonException commExc)
            {
                return BadRequest(new { Message = commExc.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Error Occurred" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TPAnnouncementDto announcementDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var announcement = await _announcementRepo.CreateAsync(announcementDto, new Guid(userId));
                return Ok(announcement);
            }
            catch (InsufficientRightsException insufExc)
            {
                return BadRequest(new { Message = insufExc.Message });
            }
            catch (CommonException commExc)
            {
                return BadRequest(new { Message = commExc.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Error Occurred" });
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TPAnnouncementDto announcementDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var announcement = await _announcementRepo.EditAsync(announcementDto, new Guid(userId));
                return Ok(announcement);
            }
            catch (InsufficientRightsException insufExc)
            {
                return BadRequest(new { Message = insufExc.Message });
            }
            catch (CommonException commExc)
            {
                return BadRequest(new { Message = commExc.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Error Occurred" });
            }
        }
    }
}