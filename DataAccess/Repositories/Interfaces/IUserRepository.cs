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
        Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> userIds);
        Task<UserDto> GetUserAsync(Guid userId);
        Task<bool> DoesUserExistsAsync(Guid userId);
    }
}
