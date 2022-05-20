using System;

namespace WebApi.Models
{
    public class TrainingSchedule
    {
        public int Id { get; set; }

        public Day Day { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }

    public class Day
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
