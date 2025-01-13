namespace ecommerce_api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime? Dateupdated { get; set; }
    }

    public class UserDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public IFormFile? Avatar { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime? Dateupdated { get; set; }
    }

    public class Userlogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
