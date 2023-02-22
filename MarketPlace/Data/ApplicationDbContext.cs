using MarketPlace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MarketPlace.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Categories> Categories { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Addresses> Addresses { get; set; }
}
