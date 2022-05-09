using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class TrainingSchedule
    {
        public TrainingSchedule()
        {
            GroupTrainingSchedules = new HashSet<GroupTrainingSchedule>();
        }

        public int Id { get; set; }
        public int? Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Day DayNavigation { get; set; }
        public virtual ICollection<GroupTrainingSchedule> GroupTrainingSchedules { get; set; }
    }
}
