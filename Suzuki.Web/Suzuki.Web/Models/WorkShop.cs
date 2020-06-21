using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suzuki.Web.Models
{
    public partial class WorkShop
    {
        public WorkShop()
        {
            WorkShopLocation = new HashSet<WorkShopLocation>();
        }

        public Guid Id { get; set; }
        [Required]
        public string WorkShopCode { get; set; }
        [Required]
        public string WorkShopName { get; set; }
        public Guid DistributerId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Distributor Distributer { get; set; }
        public virtual ICollection<WorkShopLocation> WorkShopLocation { get; set; }
    }
}
