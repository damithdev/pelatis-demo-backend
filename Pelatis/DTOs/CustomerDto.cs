using Pelatis.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.DTOs
{
    public class CustomerDto
    {
        public CustomerDto()
        {

        }

        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Phone = customer.Email;
            Business = customer.Business;
            CreatedDate = customer.CreatedDate;
            UpdatedDate = customer.UpdatedDate;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Email { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string Phone { get; set; }

        public Business Business { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
