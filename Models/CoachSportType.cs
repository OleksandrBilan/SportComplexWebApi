using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class CoachSportType
    {
        public int Id { get; set; }
        public int? SportType { get; set; }
        public int? Coach { get; set; }

        public virtual Coach CoachNavigation { get; set; }
        public virtual SportType SportTypeNavigation { get; set; }
    }
}
