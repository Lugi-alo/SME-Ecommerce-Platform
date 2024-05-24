using System;
using System.IO;
using System.Threading.Tasks;
using Dissertation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                ServiceListings = new ServiceListings();
            }
            else
            {
                ServiceListings = await _context.ServiceListings.FindAsync(id);
                if (ServiceListings == null)
                {
                    return NotFound();
                }
            }
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

                var loggedInUserId = _userManager.GetUserId(User);

                if (ServiceListings.Id == 0)
                {
                    ServiceListings.BusinessId = loggedInUserId;

                    _context.ServiceListings.Add(ServiceListings);
                    Console.WriteLine("Added new service listing.");
                }
                else
                {
                    if (ServiceListings.BusinessId == null)
                    {
                        ServiceListings.BusinessId = loggedInUserId;
                    }

                    _context.Attach(ServiceListings).State = EntityState.Modified;
                    Console.WriteLine("Modified existing service listing.");
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("Changes saved successfully.");

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the listing: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"An error occurred while saving the listing: {ex.Message}");
                return Page();
            }
        }

        private bool IsImageFile(IFormFile file)
        {
            return file.ContentType.ToLower() == "image/jpeg" ||
                   file.ContentType.ToLower() == "image/png" ||
                   file.ContentType.ToLower() == "image/gif" ||
                   file.ContentType.ToLower() == "image/svg+xml";
        }
    }
}
