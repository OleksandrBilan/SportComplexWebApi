using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class GroupTraining
    {
        public int Id { get; set; }
        public int? SubscriptionReceipt { get; set; }
        public int? Group { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Group GroupNavigation { get; set; }
        public virtual SubscriptionReceipt SubscriptionReceiptNavigation { get; set; }
    }
}
