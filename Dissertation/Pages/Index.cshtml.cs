using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dissertation.Pages
{
	public class IndexModel : PageModel
	{
		private readonly AppDataContext _context;
		private readonly UserManager<AppUser> _userManager;

		public List<ServiceListings> AllListings { get; set; }
		public bool IsAuthenticated { get; private set; }

		public IndexModel(AppDataContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			IsAuthenticated = User.Identity.IsAuthenticated;
			AllListings = await _context.ServiceListings.Include(s => s.Reviews).ToListAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostSearchAsync(string searchText, string categoryFilter)
		{
			IQueryable<ServiceListings> query = _context.ServiceListings.Include(s => s.Reviews);

			if (!string.IsNullOrEmpty(searchText))
			{
				query = query.Where(l => l.Name.ToLower().Contains(searchText.ToLower()));
			}

			if (!string.IsNullOrEmpty(categoryFilter))
			{
				query = query.Where(l => l.Category.ToLower() == categoryFilter.ToLower());
			}

			AllListings = await query.ToListAsync();

			IsAuthenticated = User.Identity.IsAuthenticated;

			return Page();
		}
	}
}

