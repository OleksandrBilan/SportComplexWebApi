using System;
using WebApi.Models.EmployeeInfo;

namespace WebApi.Models.GroupTrainingSubscription
{
    public class SubscriptionReceipt
    {
        public int Id { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsPayed { get; set; }

        public bool IsActive { get; set; }

        public Employee Seller { get; set; }

        public Customer Customer { get; set; }

        public SubscriptionType SubscriptionType { get; set; }
    }
}
