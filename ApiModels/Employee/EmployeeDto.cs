using System;

namespace WebApi.ApiModels.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? DismissDate { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int PositionId { get; set; }

        public int GymId { get; set; }
    }
}
