using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using college_assignment_mvc_project.Models;

namespace college_assignment_mvc_project.Models
{
    public class college_assignment_mvc_projectContext : DbContext
    {
        public college_assignment_mvc_projectContext (DbContextOptions<college_assignment_mvc_projectContext> options)
            : base(options)
        {
        }

        public DbSet<college_assignment_mvc_project.Models.Track> Track { get; set; }

        public DbSet<college_assignment_mvc_project.Models.Guide> Guide { get; set; }
    }
}
