using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class MembershipReceipt
    {
        public MembershipReceipt()
        {
            IndividualTrainings = new HashSet<IndividualTraining>();
        }

        public int Id { get; set; }
        public int? Customer { get; set; }
        public int? Employee { get; set; }
        public int? MembershipType { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime? PayementDateTime { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual Employee EmployeeNavigation { get; set; }
        public virtual MembershipType MembershipTypeNavigation { get; set; }
        public virtual ICollection<IndividualTraining> IndividualTrainings { get; set; }
    }
}
