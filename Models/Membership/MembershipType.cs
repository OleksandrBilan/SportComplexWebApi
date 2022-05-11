using System.Collections.Generic;

namespace WebApi.Models.Membership
{
    public class MembershipType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int AvailabilityDurationInMonths { get; set; }

        public string WorkoutStartTime { get; set; }

        public string WorkoutEndTime { get; set; }

        public List<SportType> SportTypes { get; set; }
    }
}
