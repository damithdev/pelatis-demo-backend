using Pelatis.Dto.Validators;
using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Dto
{
    public class AppUserDto
    {
        public AppUserDto()
        {

        }

        public AppUserDto(AppUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            CreatedDate = user.CreatedDate;
            UpdatedDate = user.UpdatedDate;
            IsDeleted = user.IsDeleted;
            //Password = "";


        }

        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [EmailValidator]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        //public List<Business> Businesses { get; set; }
    }
}
