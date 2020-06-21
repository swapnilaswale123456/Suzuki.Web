using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suzuki.Web.Models
{
    public partial class Distributor
    {
        public Distributor()
        {
            WorkShop = new HashSet<WorkShop>();
        }

        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        [Required]
        public string DistributorCode { get; set; }
        [Required]
        public string DistributorName { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<WorkShop> WorkShop { get; set; }
    }
}
