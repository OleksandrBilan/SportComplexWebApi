using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class SubscriptionType
    {
        public SubscriptionType()
        {
            SubscriptionReceipts = new HashSet<SubscriptionReceipt>();
        }

        public int Id { get; set; }
        public int? SportSection { get; set; }
        public int? AvailableTrainingsCount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual SportSection SportSectionNavigation { get; set; }
        public virtual ICollection<SubscriptionReceipt> SubscriptionReceipts { get; set; }
    }
}
