using Business.TravelPlan.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class TravelPlanStatusController : Controller
    {
        private readonly ITravelPlanStatusService _travelPlanStatusService;

        public TravelPlanStatusController(ITravelPlanStatusService travelPlanStatusService)
        {
            _travelPlanStatusService = travelPlanStatusService;
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var statuses = await _travelPlanStatusService.ListAsync();

                return Ok(statuses);
            }
            catch
            {
                throw;
            }
        }
    }
}