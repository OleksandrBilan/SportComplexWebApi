using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class EmployeeEducation
    {
        public int Id { get; set; }
        public int? University { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? Employee { get; set; }
        public DateTime GraduationDate { get; set; }
        public int? Level { get; set; }
        public int? Specialty { get; set; }

        public virtual Employee EmployeeNavigation { get; set; }
        public virtual EducationLevel LevelNavigation { get; set; }
        public virtual EducationSpecialty SpecialtyNavigation { get; set; }
        public virtual University UniversityNavigation { get; set; }
    }
}
