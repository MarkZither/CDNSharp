using Microsoft.EntityFrameworkCore;

namespace CDNSharp.Web.Models
{
    public class MyDataContext : DbContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options)
            : base(options)
        {
        }

        public DbSet<CDNFileInfoString> CDNFileInfoStrings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>().OwnsOne(c => c.HomeAddress).WithOwner();
            //modelBuilder.Entity<Customer>().OwnsMany(c => c.FavoriteAddresses).WithOwner();
        }
    }
}
