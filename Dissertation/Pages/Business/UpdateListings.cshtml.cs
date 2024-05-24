using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dissertation.Pages.Business
{
    public class UpdateListingsModel : PageModel
    {
        private readonly AppDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UpdateListingsModel(AppDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ServiceListings> BusinessListings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("User is null. Redirecting to authorization page.");
                return RedirectToPage("/Authorisation");
            }

            Console.WriteLine($"User ID: {user.Id}");

            BusinessListings = await _context.ServiceListings
                .Where(listing => listing.BusinessId == user.Id)
                .ToListAsync();

            Console.WriteLine($"Number of business listings found: {BusinessListings.Count}");

            ViewData["BusinessListings"] = BusinessListings;
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int listingId)
        {

            var listing = await _context.ServiceListings.FindAsync(listingId);
            if (listing == null)
            {
                return NotFound();
            }

            _context.ServiceListings.Remove(listing);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
