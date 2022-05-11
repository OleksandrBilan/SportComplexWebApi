using System;

namespace WebApi.ApiModels
{
    public class IndividualTrainingDto
    {
        public int Id { get; set; }

        public int MembershipReceiptId { get; set; }

        public int PayedHours { get; set; }

        public decimal Price { get; set; }

        public int IndividualCoachId { get; set; }

        public DateTime? PayementDateTime { get; set; }
    }
}
