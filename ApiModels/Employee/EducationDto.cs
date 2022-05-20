using System;

namespace WebApi.ApiModels.Employee
{
    public class EducationDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string University { get; set; }

        public DateTime GraduationDate { get; set; }

        public int LevelId { get; set; }

        public string Specialty { get; set; }
    }
}
