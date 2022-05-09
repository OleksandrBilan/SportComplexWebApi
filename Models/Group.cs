using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupTrainingSchedules = new HashSet<GroupTrainingSchedule>();
            GroupTrainings = new HashSet<GroupTraining>();
        }

        public int Id { get; set; }
        public int? SportSection { get; set; }
        public int? Coach { get; set; }
        public int MaxCustomersNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Coach CoachNavigation { get; set; }
        public virtual SportSection SportSectionNavigation { get; set; }
        public virtual ICollection<GroupTrainingSchedule> GroupTrainingSchedules { get; set; }
        public virtual ICollection<GroupTraining> GroupTrainings { get; set; }
    }
}
