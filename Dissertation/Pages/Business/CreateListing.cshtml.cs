using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dissertation.Models;
using Microsoft.AspNetCore.Identity;

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
            Features = await _context.Features.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile, string selectedFeatureIds)
        {
            try
            {
                // Convert selectedFeatureIds string to a list of integers
                var featureIds = selectedFeatureIds?.Split(',').Select(int.Parse).ToList();

                // Check if the featureIds is null or empty
                if (featureIds == null || !featureIds.Any())
                {
                    Console.WriteLine("No Features Selected");
                    ModelState.AddModelError("", "No features selected");
                    return Page();
                }

                // Initialize ServiceListingFeatures if it is null
                if (ServiceListings.ServiceListingFeatures == null)
                {
                    ServiceListings.ServiceListingFeatures = new List<ServiceListingFeature>();
                }

                // Set the BusinessId
                var loggedInUser = await _userManager.GetUserAsync(User);
                if (loggedInUser != null)
                {
                    ServiceListings.BusinessId = loggedInUser.Id;
                }

                // Check if the image file is provided
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

                // Add the ServiceListings entity to the context to generate Id
                _context.ServiceListings.Add(ServiceListings);
                await _context.SaveChangesAsync();

                // Check if the ServiceListings.Id is generated
                if (ServiceListings.Id == 0)
                {
                    ModelState.AddModelError("", "ServiceListings Id not generated");
                    return Page();
                }

                // Add selected features to the ServiceListingFeatures
                foreach (var featureId in featureIds)
                {
                    var feature = await _context.Features.FindAsync(featureId);
                    if (feature != null)
                    {
                        ServiceListings.ServiceListingFeatures.Add(new ServiceListingFeature
                        {
                            ServiceListingsId = ServiceListings.Id,
                            FeaturesId = featureId
                        });
                    }
                }

                // Save changes including the associated features
                await _context.SaveChangesAsync();

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving the listing: {ex.Message}");
                return Page();
            }
        }
    }
}
