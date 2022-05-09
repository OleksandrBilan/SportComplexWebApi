using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class IndividualTraining
    {
        public int Id { get; set; }
        public int? MembershipReceipt { get; set; }
        public int PayedHours { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? IndividualCoach { get; set; }
        public DateTime? PayementDateTime { get; set; }

        public virtual IndividualCoach IndividualCoachNavigation { get; set; }
        public virtual MembershipReceipt MembershipReceiptNavigation { get; set; }
    }
}
