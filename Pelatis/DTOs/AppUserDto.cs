using Pelatis.Data.Entity;
using Pelatis.Dto.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pelatis.Dto
{
    public class AppUserDto
    {
        public AppUserDto()
        {

        }

        public AppUserDto(AppUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            CreatedDate = user.CreatedDate;
            UpdatedDate = user.UpdatedDate;
            IsDeleted = user.IsDeleted;
            DefaultBusinessId = user.DefaultBusiness;
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

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
        public int DefaultBusinessId { get; set; }
        public string Token { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
