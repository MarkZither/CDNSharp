using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDNSharp.Web.DataAccess
{
    public class MyDataContext : DbContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options)
            : base(options)
        {
        }

        [Table("_files")]
        public class CDNFile
        {
            public string Id { get; set; }

            public string filename { get; set; }

        }

        public DbSet<CDNFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CDNFile>().ToTable("_files");// OwnsOne(c => c.HomeAddress).WithOwner();
            //modelBuilder.Entity<Customer>().OwnsMany(c => c.FavoriteAddresses).WithOwner();
        }
    }
}
