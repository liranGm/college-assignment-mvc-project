using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace college_assignment_mvc_project.Models
{
    public class Track
    {
        public int TrackID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Difficulty { get; set; }

        public string Image { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public int Rate { get; set; }

        public bool Includes_Water { get; set; }

        public bool Circular { get; set; }

        public int Duration { get; set; } // In minutes

        public int TrackLenght { get; set; } // In KM

        public int GuideId { get; set; }

        public virtual Guide Guide { get; set; }
    }
}
