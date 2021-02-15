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
    [Authorize]
    public class TravelPlanController : Controller
    {
        private readonly ITravelPlanRepository _travelPlanRepository;

        public TravelPlanController(ITravelPlanRepository travelPlanRepository)
        {
            _travelPlanRepository = travelPlanRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TravelPlanDto travelPlanDto)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                await _travelPlanRepository.CreateAsync(travelPlanDto, userId);
                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest();
            }
        }
    }
}