using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Day
    {
        public Day()
        {
            TrainingSchedules = new HashSet<TrainingSchedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<TrainingSchedule> TrainingSchedules { get; set; }
    }
}
