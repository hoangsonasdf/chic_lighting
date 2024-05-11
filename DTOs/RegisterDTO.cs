using System.ComponentModel.DataAnnotations;

namespace chic_lighting.DTOs
{
    public class RegisterDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
