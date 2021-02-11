using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public string TestAuth()
        {
            return "Secret Message";
        }
        
    }
}
