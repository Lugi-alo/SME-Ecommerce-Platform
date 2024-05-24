using Dissertation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dissertation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDataContext _context;

        public DashboardModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public List<string> AllRoles { get; set; }
        public List<UserWithRoles> UsersWithRoles { get; set; }
        public List<Review> Reviews { get; set; }

        public class UserWithRoles
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public List<string> Roles { get; set; }
        }

        public async Task OnGetAsync()
        {
            UsersWithRoles = new List<UserWithRoles>();
            AllRoles = await GetAllRolesAsync();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersWithRoles.Add(new UserWithRoles
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            Reviews = await _context.Reviews.ToListAsync();
        }

        public async Task<IActionResult> OnGetDeleteUserAsync(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToPage();
            }
            else
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }
        }

        public async Task<IActionResult> OnPostToggleAccountStatusAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Page();
            }
            user.LockoutEnabled = !user.LockoutEnabled;
            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(role => role.Name).ToListAsync();
        }
    }
}
