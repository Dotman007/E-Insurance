using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace E_Insurance.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Compliance> Compliances { get; set; }
    }
}