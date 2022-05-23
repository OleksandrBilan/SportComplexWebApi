using System;

namespace WebApi.ApiModels.Employee
{
    public class PreviousJobDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Company { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
