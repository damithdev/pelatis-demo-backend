using Pelatis.Dto.Validators;
using System.ComponentModel.DataAnnotations;

namespace Pelatis.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailValidator]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
