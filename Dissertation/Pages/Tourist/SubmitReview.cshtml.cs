using Dissertation.Migrations;
using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Python.Runtime;

namespace Dissertation.Pages.Tourist
{
    public class SubmitReviewModel : PageModel
    {
        private readonly AppDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        [BindProperty]
        public Review Review { get; set; }

        public SubmitReviewModel(AppDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var serviceListing = _context.ServiceListings.FirstOrDefault(s => s.Id == id);

            if (serviceListing == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                UserId = user.Id,
                ReviewText = Review.ReviewText,
                Rating = Review.Rating,
                Timestamp = DateTime.Now,
                ServiceId = serviceListing.Id,
                ServiceListings = serviceListing
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            PythonEngine.Initialize();

            using (Py.GIL()) 
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\ejolo\OneDrive\Desktop\SME-Ecommerce-Platform\Dissertation");

                dynamic sentimentAnalysisScript = Py.Import("sentimentAnalyser");
                dynamic analysisResult = sentimentAnalysisScript.analyseText(review.ReviewText);

                var tokens = analysisResult[0].AsManagedObject(typeof(string[]));
                var sentimentScore = analysisResult[1].AsManagedObject(typeof(double));

                review.SentimentScore = sentimentScore;
                _context.SaveChanges();
            }
            return RedirectToPage("/Index");
        }
    }
}
