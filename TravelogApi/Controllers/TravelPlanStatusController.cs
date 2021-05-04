using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class TravelPlanStatusController : Controller
    {
        private readonly ITravelPlanStatusRepository _travelPlanStatusRepository;

        public TravelPlanStatusController(ITravelPlanStatusRepository travelPlanStatusRepository)
        {
            _travelPlanStatusRepository = travelPlanStatusRepository;
        }
        public async Task<IActionResult> List()
        {
            try
            {
                var statuses = await _travelPlanStatusRepository.List();

                return Ok(statuses);
            }
            catch
            {
                throw; 
            }

        }
        
    }
}
