using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categories>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cart>()
         .HasOne(c => c.User)
         .WithMany()
         .HasForeignKey(c => c.UserId)
         .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Addresses>(entity =>
        {
            entity.Property(e => e.Country).HasMaxLength(30);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
        });

        modelBuilder.Entity<ShippingType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(70);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(20);
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(70);
        });

        modelBuilder.Entity<ApplicationUsers>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
        });

        modelBuilder.Entity<AddressesForOrders>(entity =>
        {
            entity.Property(e => e.Country).HasMaxLength(30);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
        });
    }

    public DbSet<Categories> Categories { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Addresses> Addresses { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<ShippingType> ShippingTypes { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<AddressesForOrders> AddressesForOrders { get; set; }
}
