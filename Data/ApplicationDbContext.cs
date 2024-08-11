namespace PhoneDirectory.Data
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>()
                .HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Address>()
                .HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Image>()
                .HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Contact>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            builder.Entity<Image>()
                .HasOne(x => x.Contact)
                .WithOne()
                .HasForeignKey<Image>(x => x.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Country>()
                .HasIndex(c => c.CountryPrefix)
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(c => c.IsoCode)
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
