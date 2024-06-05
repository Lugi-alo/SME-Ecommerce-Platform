using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Dissertation.Pages.Business
{
    public class CreateListingModel : PageModel
    {
        private readonly AppDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CreateListingModel(UserManager<AppUser> userManager, AppDataContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ServiceListings ServiceListings { get; set; }

        public List<Features> Features { get; set; }

        public List<SelectListItem> ListingTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Accommodation", Text = "Accommodation" },
            new SelectListItem { Value = "Restaurant", Text = "Restaurant" },
        };

        public async Task OnGetAsync()
        {
            Console.WriteLine("Function calledd");
            ServiceListings = new ServiceListings();
            Features = await _context.Features.ToListAsync();
            foreach (var feature in Features)
            {
                Console.WriteLine($"Feature: {feature.Name}, IconPath: {feature.IconPath}");
            }
            Console.WriteLine("no featuees present");

        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile, List<int> selectedFeatureIds)
        {
            try
            {
                if (selectedFeatureIds == null || !selectedFeatureIds.Any())
                {
                    Console.WriteLine("At least one feature must be selected.");
                    return Page();
                }

                ServiceListings = new ServiceListings();

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    ServiceListings.Image = "/images/" + fileName;
                }

                var loggedInUser = await _userManager.GetUserAsync(User);
                if (loggedInUser != null)
                {
                    ServiceListings.BusinessId = loggedInUser.Id;
                }

                ServiceListings.Features = new List<Features>();

                var selectedFeatures = _context.Features.Where(f => selectedFeatureIds.Contains(f.Id)).ToList();
                ServiceListings.Features.AddRange(selectedFeatures);

                _context.ServiceListings.Add(ServiceListings);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the listing: {ex.Message}");
                return Page();
            }
        }
    }
}
