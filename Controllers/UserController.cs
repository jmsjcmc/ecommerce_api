using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ecommerce_api.Data;
using ecommerce_api.Models;

namespace ecommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GET-ALL-USERS")]
        public async Task<ActionResult<IEnumerable<UserDto>>> getUsers()
        {
            var users = await _context.User.ToListAsync();
            return Ok(users.Select(u => new UserDto
            {
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Username = u.Username,
                Role = u.Role,
                Password = u.Password,
                Datereceived = u.Datecreated,
                Dateupdated = u.Dateupdated
            }).ToList());
        }

        [HttpGet("GET-USER/{id}")]
        public async Task<ActionResult<User>> getUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("REGISTER")]
        public async Task<ActionResult<User>> createUser(UserDto userDto)
        {
            var user = new User
            {
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Email = userDto.Email,
                Username = userDto.Username,
                Role = "Consumer",
                Password = BCrypt.Net.BCrypt.HashPassword (userDto.Password),
                Datecreated = DateTime.Now,
                Dateupdated = userDto.Dateupdated
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(getUser), new { id = user.Id }, user);
        }

        [HttpPut("UPDATE/{id}")]
        public async Task<ActionResult> updateUser(int id, UserDto userDto)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Firstname = userDto.Firstname;
            user.Lastname = userDto.Lastname;
            user.Email = userDto.Email;
            user.Username = userDto.Username;
            user.Role = userDto.Role;
            user.Dateupdated = DateTime.Now;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.User.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
