using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
            AppUser = business.AppUser;
            CreatedDate = business.CreatedDate;
            UpdatedDate = business.UpdatedDate;
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

        public AppUser AppUser { get; set; }

        public List<Customer> Customers { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
