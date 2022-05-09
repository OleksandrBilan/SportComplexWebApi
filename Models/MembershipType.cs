using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class MembershipType
    {
        public MembershipType()
        {
            MembershipReceipts = new HashSet<MembershipReceipt>();
            MembershipTypeSportTypes = new HashSet<MembershipTypeSportType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailibilityDurationInMonths { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime WorkoutStartTime { get; set; }
        public DateTime WorkoutEndTime { get; set; }

        public virtual ICollection<MembershipReceipt> MembershipReceipts { get; set; }
        public virtual ICollection<MembershipTypeSportType> MembershipTypeSportTypes { get; set; }
    }
}
