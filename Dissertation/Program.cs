using Dissertation.Models;
//using Dissertation.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services
builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

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

// Add the FeatureService
//builder.Services.AddScoped<FeatureService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Seed roles and features (optional)
using var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
//var featureService = scope.ServiceProvider.GetRequiredService<FeatureService>();

await SeedRolesAsync(roleManager);
//await featureService.InsertHardcodedFeaturesAsync();

app.Run();

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    var roles = new[]
    {
        "Admin",
        "BusinessOwner",
        "RegularUser"
    };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
}
