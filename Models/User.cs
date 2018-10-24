using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace college_assignment_mvc_project.Models
{
    public class User
{
    public int UserID { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public virtual ICollection<Order> OrderedTrips { get; set; } // each user might have multiple orders
    }
}
