using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Utils;

public static class InitializeUsersAndRoles
{
    public static async Task AddRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var roleName in Role.All)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                await _roleManager.CreateAsync(role);
            }
        }
    }

    public static async Task AddUsersAsync(this WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUsers>>();

        var adminUser = await _userManager.FindByEmailAsync(configuration["Username"]);
        if (adminUser is null)
        {
            adminUser = new ApplicationUsers()
            {
                UserName = configuration["Username"],
                Email = configuration["Email"],
                FirstName = configuration["FirstName"],
                LastName = configuration["LastName"],
                PhoneNumber = configuration["PhoneNumber"]
            };
            await _userManager.CreateAsync(adminUser, configuration["Password"]);
            app.Logger.LogInformation($"{configuration["Username"]} user added");
        }

        if (!await _userManager.IsEmailConfirmedAsync(adminUser))
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
            var result = await _userManager.ConfirmEmailAsync(adminUser, token);

            if (!result.Succeeded)
            {
                app.Logger.LogInformation($"{configuration["Username"]} wasn't confirmed!");
            }
            else
            {
                await _userManager.AddToRoleAsync(adminUser, Role.Admin);
                app.Logger.LogInformation($"{configuration["Username"]} with role {Role.Admin} was created successfully");
            }
        }
    }
    public static async Task AddPaymentMethods(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (!_db.PaymentTypes.Any())
        {
            var paymentTypes = new List<PaymentType>()
            {
                new PaymentType  {Name="Privat24", Description="2% tax" },
                new PaymentType {Name="MonoBank", Description="0% tax"},
                new PaymentType {Name="Cash", Description="Just cash"}
            };
            await _db.PaymentTypes.AddRangeAsync(paymentTypes);
            await _db.SaveChangesAsync();
        }
    }
    public static async Task AddCategories(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (!_db.Categories.Any())
        {
            var categories = new List<Categories>()
            {
                new Categories{Name = "Electronics"},
                new Categories{Name = "For home"},
                new Categories{Name = "Clothes"}
            };
            await _db.Categories.AddRangeAsync(categories);
            await _db.SaveChangesAsync();
        }
    }
    public static async Task AddShippingTypes(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (!_db.ShippingTypes.Any())
        {
            var shippingTypes = new List<ShippingType>()
            {
                new ShippingType{Name = "Nova Poshta"},
                new ShippingType{Name = "Urk Poshta"},
                new ShippingType{Name = "Pickup"},
            };
            await _db.ShippingTypes.AddRangeAsync(shippingTypes);
            await _db.SaveChangesAsync();
        }
    }
}
