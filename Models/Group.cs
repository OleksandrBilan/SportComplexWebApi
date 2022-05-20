using System;
using System.Collections.Generic;
using WebApi.Models.CoachInfo;
using WebApi.Models.GroupTrainingSubscription;

namespace WebApi.Models
{
    public class Group
    {
        public int Id { get; set; }

        public SportSection SportSection { get; set; }

        public Coach Coach { get; set; }

        public int MaxCustomersCount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<TrainingSchedule> Schedules { get; set; }
    }
}
