using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Gym
    {
        public Gym()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public int? City { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual City CityNavigation { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
