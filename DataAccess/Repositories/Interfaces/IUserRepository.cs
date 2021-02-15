using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetAsync(Guid userId);
        Task<bool> CreateAsync(UserDto user);
        Task<bool> EditAsync(UserDto user);
        Task<bool> DeleteAsync(Guid userId);

    }
}
