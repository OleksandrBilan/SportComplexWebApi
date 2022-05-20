using System;
using WebApi.Models.GroupTrainingSubscription;

namespace WebApi.Models
{
    public class GroupTraining
    {
        public int Id { get; set; }

        public SubscriptionReceipt Receipt { get; set; }

        public Group Group { get; set; }

        public DateTime StartDateTime { get; set; }
    }
}
