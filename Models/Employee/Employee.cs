using System;

namespace WebApi.Models.Employee
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? DismissDate { get; set; }
        
        public string Login { get; set; }

        public string Password { get; set; }

        public PositionType Position { get; set; }

        public Gym Gym { get; set; }
    }
}
