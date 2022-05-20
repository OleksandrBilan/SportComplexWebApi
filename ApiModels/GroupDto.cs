using System;
using System.Collections.Generic;

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

        public List<TrainingScheduleDto> Schedules { get; set; }
    }

    public class TrainingScheduleDto
    {
        public int Id { get; set; }

        public int DayId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}
