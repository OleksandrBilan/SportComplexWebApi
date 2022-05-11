using System.Collections.Generic;

namespace WebApi.ApiModels
{
    public class CoachDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Description { get; set; }

        public List<int> SportTypeIds { get; set; }

        public bool CanBeIndividual { get; set; }

        public decimal? PricePerHour { get; set; }
    }
}
