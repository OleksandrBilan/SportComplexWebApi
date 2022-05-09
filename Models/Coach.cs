using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class Coach
    {
        public Coach()
        {
            CoachSportTypes = new HashSet<CoachSportType>();
            Groups = new HashSet<Group>();
            IndividualCoaches = new HashSet<IndividualCoach>();
        }

        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual ICollection<CoachSportType> CoachSportTypes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<IndividualCoach> IndividualCoaches { get; set; }
    }
}
