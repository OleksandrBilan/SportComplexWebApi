using System;
using WebApi.Models.EmployeeInfo;

namespace WebApi.Models.Membership
{
    public class MembershipReceipt
    {
        public int Id { get; set; }

        public Employee Seller { get; set; }

        public Customer Customer { get; set; }

        public MembershipType MembershipType { get; set; }

        public DateTime PayementDateTime { get; set; }
    }
}
