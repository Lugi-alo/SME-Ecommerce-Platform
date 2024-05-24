using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Python.Runtime;


var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

Runtime.PythonDLL = @"C:\Users\ejolo\AppData\Local\Programs\Python\Python39\python39.dll";

builder.Services.AddRazorPages();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Configure password options
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<AppDataContext>();

// Add the TextAnalyser service
builder.Services.AddScoped<TextAnalyser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(" / Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Seed roles (optional)
using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

await SeedRolesAsync(roleManager);

app.Run();

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    // Define roles
    var roles = new[]
    {
        "Admin",
        "BusinessOwner",
        "RegularUser"
    };

    foreach (var roleName in roles)
    {
        // Check if the role exists
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            // Create the role if it doesn't exist
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
}
