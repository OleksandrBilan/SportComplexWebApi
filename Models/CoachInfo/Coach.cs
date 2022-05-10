using System.Collections.Generic;
using WebApi.Models.EmployeeInfo;

namespace WebApi.Models.CoachInfo
{
    public class Coach
    {
        public int Id { get; set; }

        public Employee EmployeeInfo { get; set; }

        public string Description { get; set; }
        
        public List<SportType> SportTypes { get; set; }
    }
}
