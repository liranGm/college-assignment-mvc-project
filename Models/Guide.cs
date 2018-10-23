using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace college_assignment_mvc_project.Models
{
    public class Guide
    {
        public int GuideID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int PricePerDay { get; set; }

        public int Rate { get; set; }

        public virtual List<Track> Tracks { get; set; }
    }
}
