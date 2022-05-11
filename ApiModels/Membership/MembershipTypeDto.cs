using System.Collections.Generic;

namespace WebApi.ApiModels.Membership
{
    public class MembershipTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int AvailabilityDurationInMonths { get; set; }

        public string WorkoutStartTime { get; set; }

        public string WorkoutEndTime { get; set; }

        public List<int> SportTypeIds { get; set; }
    }
}
