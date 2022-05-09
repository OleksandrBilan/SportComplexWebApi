using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Models
{
    public partial class City
    {
        public City()
        {
            Gyms = new HashSet<Gym>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<Gym> Gyms { get; set; }
    }
}
