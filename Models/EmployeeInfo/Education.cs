using System;

namespace WebApi.Models.EmployeeInfo
{
    public class Education
    {
        public int Id { get; set; }

        public University University { get; set; }

        public DateTime GraduationDate { get; set; }

        public EducationLevel Level { get; set; }

        public EducationSpecialty Specialty { get; set; }
    }
}
