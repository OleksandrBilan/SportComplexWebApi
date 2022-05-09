using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class PreviousJob
    {
        public int Id { get; set; }
        public int? Employee { get; set; }
        public int? Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Company CompanyNavigation { get; set; }
        public virtual Employee EmployeeNavigation { get; set; }
    }
}
