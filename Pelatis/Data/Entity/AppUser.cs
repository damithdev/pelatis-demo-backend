using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string LastName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Email { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public byte[] Secret { get; set; }

        public byte[] Salt { get; set; }

        public List<Business> Businesses { get; set; }

        public int DefaultBusiness { get; set; }
    }
}
