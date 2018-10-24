using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace college_assignment_mvc_project.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public double TotalPrice { get; set; }
        public int PurchasedTrackID { get; set; }
        public int SelectedGuildeID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime TripsDate { get; set; }

        public virtual User customer { get; set; } // the customer who bought this trip
    }
}
