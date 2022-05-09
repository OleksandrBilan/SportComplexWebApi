using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class GroupTrainingSchedule
    {
        public int Id { get; set; }
        public int? Group { get; set; }
        public int? TrainingSchedule { get; set; }

        public virtual Group GroupNavigation { get; set; }
        public virtual TrainingSchedule TrainingScheduleNavigation { get; set; }
    }
}
