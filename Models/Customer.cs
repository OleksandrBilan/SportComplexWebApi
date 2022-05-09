using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Customer
    {
        public Customer()
        {
            MembershipReceipts = new HashSet<MembershipReceipt>();
            SubscriptionReceipts = new HashSet<SubscriptionReceipt>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<MembershipReceipt> MembershipReceipts { get; set; }
        public virtual ICollection<SubscriptionReceipt> SubscriptionReceipts { get; set; }
    }
}
