using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class SubscriptionReceipt
    {
        public SubscriptionReceipt()
        {
            GroupTrainings = new HashSet<GroupTraining>();
        }

        public int Id { get; set; }
        public int? Employee { get; set; }
        public int? Customer { get; set; }
        public int? SubscriptionType { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsPayed { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public bool IsActive { get; set; }

        public virtual Customer CustomerNavigation { get; set; }
        public virtual Employee EmployeeNavigation { get; set; }
        public virtual SubscriptionType SubscriptionTypeNavigation { get; set; }
        public virtual ICollection<GroupTraining> GroupTrainings { get; set; }
    }
}
