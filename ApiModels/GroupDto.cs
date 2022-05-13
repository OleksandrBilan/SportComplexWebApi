using System;

namespace WebApi.ApiModels
{
    public class GroupDto
    {
        public int Id { get; set; }

        public int SportSectionId { get; set; }

        public int CoachId { get; set; }

        public int MaxCustomersNumber { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
