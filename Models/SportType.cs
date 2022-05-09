using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class SportType
    {
        public SportType()
        {
            CoachSportTypes = new HashSet<CoachSportType>();
            MembershipTypeSportTypes = new HashSet<MembershipTypeSportType>();
            SportSections = new HashSet<SportSection>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<CoachSportType> CoachSportTypes { get; set; }
        public virtual ICollection<MembershipTypeSportType> MembershipTypeSportTypes { get; set; }
        public virtual ICollection<SportSection> SportSections { get; set; }
    }
}
