using System;

namespace WebApi.Models.EmployeeInfo
{
    public class PreviousJob
    {
        public int Id { get; set; }

        public Company Company { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
