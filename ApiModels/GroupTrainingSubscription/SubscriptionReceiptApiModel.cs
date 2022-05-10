using System;

namespace WebApi.ApiModels.GroupTrainingSubscription
{
    public class SubscriptionReceiptApiModel
    {
        public int Id { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsPayed { get; set; }

        public bool IsActive { get; set; }

        public int SellerId { get; set; }

        public int CustomerId { get; set; }

        public int SubscriptionTypeId { get; set; }
    }
}
