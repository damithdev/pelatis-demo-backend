using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Entities
{
    public class Business
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string TypeOfBusiness { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Country { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Currency { get; set; }

        public virtual AppUser AppUser { get; set; }

        public List<Customer> Customers { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedDate { get; set; }
    }
}
