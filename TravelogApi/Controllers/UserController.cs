using DataAccess.Repositories.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelogApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Get([FromQuery] Guid userId)
        {
            try
            {
                var user = await _userRepository.GetAsync(userId);
                return Ok(user);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto user)
        {
            try
            {
                var isSuccessful = await _userRepository.CreateAsync(user);

                return isSuccessful ? StatusCode(200) : StatusCode(500);

            }
            catch
            {
                return StatusCode(500);

            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UserDto user)
        {
            try
            {
                var isSuccessful = await _userRepository.EditAsync(user);

                return isSuccessful ? StatusCode(200) : StatusCode(500);

            }
            catch
            {
                return StatusCode(500);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] Guid userId)
        {
            try
            {
                var isSuccessful = await _userRepository.DeleteAsync(userId);

                return isSuccessful ? StatusCode(200) : StatusCode(500);

            }
            catch
            {
                return StatusCode(500);

            }

        }

    }
}
