using Pelatis.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pelatis.DTOs
{
    public class BusinessDto
    {
        public BusinessDto()
        {

        }

        public BusinessDto(Business business)
        {
            Id = business.Id;
            CompanyName = business.CompanyName;
            TypeOfBusiness = business.TypeOfBusiness;
            Country = business.Country;
            Currency = business.Currency;
            CreatedDate = business.CreatedDate;
            UpdatedDate = business.UpdatedDate;

            if (business.AppUser != null)
            {
                AppUserId = business.AppUser.Id;

            }
        }

        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string TypeOfBusiness { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Country { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Currency { get; set; }

        [JsonIgnore]
        public int AppUserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
