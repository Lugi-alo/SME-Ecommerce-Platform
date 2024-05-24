using Dissertation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dissertation.Pages.Tourist
{
    public class ViewListingModel : PageModel
    {
        private readonly AppDataContext _context;

        public ServiceListings ServiceListing { get; set; }
        public List<Review> Reviews { get; set; }

        public ViewListingModel(AppDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int serviceId)
        {
            try
            {
                ServiceListing = await _context.ServiceListings
                    .Include(sl => sl.Reviews)
                    .ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(sl => sl.Id == serviceId);

                if (ServiceListing == null)
                {
                    return NotFound();
                }

                Reviews = ServiceListing.Reviews.ToList();

                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostLikeReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            review.LikeCount++;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { serviceId = review.ServiceId });
        }
    }
}
