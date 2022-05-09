using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class IndividualCoach
    {
        public IndividualCoach()
        {
            IndividualTrainings = new HashSet<IndividualTraining>();
        }

        public int Id { get; set; }
        public int? Coach { get; set; }
        public decimal PricePerHour { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Coach CoachNavigation { get; set; }
        public virtual ICollection<IndividualTraining> IndividualTrainings { get; set; }
    }
}
