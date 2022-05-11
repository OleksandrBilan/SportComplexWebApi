using System;
using WebApi.Models.CoachInfo;
using WebApi.Models.Membership;

namespace WebApi.Models
{
    public class IndividualTraining
    {
        public int Id { get; set; }

        public MembershipReceipt MembershipReceipt { get; set; }

        public int PayedHours { get; set; }

        public decimal Price { get; set; }

        public IndividualCoach Coach { get; set; }

        public DateTime? PayementDateTime { get; set; }
    }
}
