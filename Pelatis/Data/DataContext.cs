using Microsoft.EntityFrameworkCore;
using Pelatis.Data.Entity;

namespace Pelatis.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasIndex(p => new
            {
                p.Email
            }).IsUnique();

            //modelBuilder.Entity<AppUser>().HasData(
            //    new AppUser
            //    {
            //        FirstName = "Damith",
            //        LastName = "Warnakulasuriya",
            //        Email = "sendtodamith@gmail.com",
            //    });
        }

    }
}
