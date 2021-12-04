using Pelatis.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
            CreatedDate = customer.CreatedDate;
            UpdatedDate = customer.UpdatedDate;

            if (customer.Business != null)
            {
                BusinessId = customer.Business.Id;
            }
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

        public int BusinessId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
