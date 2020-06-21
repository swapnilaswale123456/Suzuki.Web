using System;
using System.Collections.Generic;

namespace Suzuki.Web.Models
{
    public partial class WorkShopLocation
    {
        public Guid Id { get; set; }
        public Guid WorkShopId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual WorkShop WorkShop { get; set; }
    }
}
