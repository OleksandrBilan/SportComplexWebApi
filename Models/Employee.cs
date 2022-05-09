using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Coaches = new HashSet<Coach>();
            EmployeeEducations = new HashSet<EmployeeEducation>();
            MembershipReceipts = new HashSet<MembershipReceipt>();
            PreviousJobs = new HashSet<PreviousJob>();
            SubscriptionReceipts = new HashSet<SubscriptionReceipt>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int? Position { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime HireDate { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? DismissDate { get; set; }
        public int? Gym { get; set; }

        public virtual Gym GymNavigation { get; set; }
        public virtual PositionType PositionNavigation { get; set; }
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }
        public virtual ICollection<MembershipReceipt> MembershipReceipts { get; set; }
        public virtual ICollection<PreviousJob> PreviousJobs { get; set; }
        public virtual ICollection<SubscriptionReceipt> SubscriptionReceipts { get; set; }
    }
}
