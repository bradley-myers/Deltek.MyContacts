using Deltek.MyContacts.Models;
using Microsoft.EntityFrameworkCore;

namespace Deltek.MyContacts.Data
{
    public class MyContactsDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        
        public MyContactsDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Category>().HasMany(c => c.ChildCategories).WithOne(c => c.ParentCategory);
        }
    }
}