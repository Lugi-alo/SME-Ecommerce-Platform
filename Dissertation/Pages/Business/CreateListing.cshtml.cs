using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dissertation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public List<SelectListItem> ListingTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Accommodation", Text = "Accommodation" },
            new SelectListItem { Value = "Restaurant", Text = "Restaurant" },
        };

        // This property will hold the selected features for the service listing
        [BindProperty]
        public List<int> SelectedFeatureIds { get; set; }

        // Populate the features dropdown list
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Title"] = "Add Service Listings";
            ServiceListings = new ServiceListings();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imageFile)
        {
            try
            {
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

                // Add selected features to the service listing
                if (SelectedFeatureIds != null)
                {
                    ServiceListings.Features = await _context.Features.Where(f => SelectedFeatureIds.Contains(f.Id)).ToListAsync();
                }

                _context.ServiceListings.Add(ServiceListings);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while saving the listing: {ex.Message}");
                return Page();
            }
        }
    }
}
