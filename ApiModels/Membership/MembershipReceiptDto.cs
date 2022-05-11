using System;

namespace WebApi.ApiModels.Membership
{
    public class MembershipReceiptDto
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public int CustomerId { get; set; }

        public int MembershipTypeId { get; set; }

        public DateTime PayementDateTime { get; set; }
    }
}
