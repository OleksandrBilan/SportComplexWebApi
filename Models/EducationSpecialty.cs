using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class EducationSpecialty
    {
        public EducationSpecialty()
        {
            EmployeeEducations = new HashSet<EmployeeEducation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }
    }
}
