using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class PositionType
    {
        public PositionType()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
