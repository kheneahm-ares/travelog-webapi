using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ITravelPlanStatusRepository
    {
        Task<List<TravelPlanStatusDto>> List(); 
    }
}
