using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ecommerce_api.Data;
using ecommerce_api.Models;
using ecommerce_api.Services;

namespace ecommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GET-ALL-USERS")]
        public async Task<ActionResult<IEnumerable<UserDto>>> getUsers()
        {
           var users = await _userService.getAllUsers();
            return Ok(users);
        }

        [HttpGet("GET-USER/{id}")]
        public async Task<ActionResult<User>> getUser(int id)
        {
            var users = await _userService.getUserById(id);
            if (users == null)
            {
                return NotFound();
            }
            return users;
        }

        [HttpPost("REGISTER")]
        public async Task<ActionResult<User>> createUser(UserDto userDto)
        {
            var user = await _userService.createUser(userDto);
            return CreatedAtAction(nameof(getUser), new { id = user.Id }, user);
        }

        [HttpPut("UPDATE/{id}")]
        public async Task<ActionResult> updateUser(int id, UserDto userDto)
        {
            var user = await _userService.updateUser(id, userDto);
            if (!user)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _userService.deleteUser(id);
            if(!user)
                return NotFound();
            return NoContent();
        }
    }
}
