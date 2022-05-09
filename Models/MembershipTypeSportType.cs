using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class MembershipTypeSportType
    {
        public int Id { get; set; }
        public int? MembershipType { get; set; }
        public int? SportType { get; set; }

        public virtual MembershipType MembershipTypeNavigation { get; set; }
        public virtual SportType SportTypeNavigation { get; set; }
    }
}
