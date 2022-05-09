using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class SportSection
    {
        public SportSection()
        {
            Groups = new HashSet<Group>();
            SubscriptionTypes = new HashSet<SubscriptionType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? SportType { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual SportType SportTypeNavigation { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
    }
}
