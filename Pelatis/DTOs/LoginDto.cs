using Pelatis.Dto.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
