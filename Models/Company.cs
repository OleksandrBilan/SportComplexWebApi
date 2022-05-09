using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Company
    {
        public Company()
        {
            PreviousJobs = new HashSet<PreviousJob>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<PreviousJob> PreviousJobs { get; set; }
    }
}
