using ecommerce_api.Data;
using ecommerce_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_api.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public static UserDto mapToDto(User user)
        {
            return new UserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                Password = user.Password,
                Datecreated = user.Datecreated,
                Dateupdated = user.Dateupdated
            };
        }

        public async Task<List<UserDto>> getAllUsers()
        {
            var users = await _context.User.ToListAsync();
            return users.Select(mapToDto).ToList();
        }

        public async Task<User> getUserById(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> createUser(UserDto userDto)
        {
          
            var avatarPath = await Saveavatar(userDto.Avatar);
            var user = new User
            {
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Email = userDto.Email,
                Username = userDto.Username,
                Role = "Consumer",
                Avatar = avatarPath,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Datecreated = DateTime.Now,
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> updateUser(int id, UserDto userDto)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return false;

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
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.User.AnyAsync(e => e.Id == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> deleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return false;
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string?> Saveavatar(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;
            var avatarPath = @"C:\Users\jatabilog\Desktop\xxx\Ecommerce Client\src\assets\avatars";

            if (!Directory.Exists(avatarPath))
            {
                Directory.CreateDirectory(avatarPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(avatarPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"assets/avatars/{uniqueFileName}";
        }
    }
}
