using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suzuki.Web.Models
{
    public partial class Country
    {
        public Country()
        {
            Distributor = new HashSet<Distributor>();
        }

        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string CountryName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string Langauge { get; set; }

        public virtual ICollection<Distributor> Distributor { get; set; }
    }
}
