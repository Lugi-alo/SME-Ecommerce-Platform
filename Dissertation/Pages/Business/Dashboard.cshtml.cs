using System;
using System.Linq;
using Dissertation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Dissertation.Pages.Business
{
    public class DashboardModel : PageModel
    {
        private readonly AppDataContext _context;

        public DashboardModel(AppDataContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            LoadDefaultData();
        }

        [HttpPost]
        public IActionResult OnPostSearch(string listingName)
        {
            Console.WriteLine("OnPostSearch method called."); 
            var service = _context.ServiceListings.FirstOrDefault(s => s.Name == listingName);

            Console.Write(service);

            if (service == null)
            {
                LoadDefaultData();
                return Page();
            }

            var reviews = _context.Reviews.Where(r => r.ServiceId == service.Id).ToList();

            var sentimentData = reviews
                .GroupBy(r => new { Year = r.Timestamp.Year, Month = r.Timestamp.Month, Day = r.Timestamp.Day })
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                    AverageSentimentScore = g.Average(r => r.SentimentScore)
                })
                .OrderBy(g => g.Date)
                .ToList();

            var sentimentLabels = sentimentData.Select(d => d.Date.ToString("MMM dd, yyyy")).ToList();
            var sentimentScores = sentimentData.Select(d => d.AverageSentimentScore).ToList();

            var ratingsCount = reviews
                .GroupBy(r => r.Rating)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            var ratingLabels = Enumerable.Range(1, 5).Select(i => i.ToString()).ToList();
            var ratingData = ratingsCount;

            return new JsonResult(new
            {
                SentimentChartData = new { labels = sentimentLabels, data = sentimentScores },
                RatingChartData = new { labels = ratingLabels, data = ratingData }
            });
        }

        private void LoadDefaultData()
        {
            var reviews = _context.Reviews.ToList();

            var sentimentData = reviews
                .GroupBy(r => new { Year = r.Timestamp.Year, Month = r.Timestamp.Month, Day = r.Timestamp.Day })
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                    AverageSentimentScore = g.Average(r => r.SentimentScore)
                })
                .OrderBy(g => g.Date)
                .ToList();

            var sentimentLabels = sentimentData.Select(d => d.Date.ToString("MMM dd, yyyy")).ToList();
            var sentimentScores = sentimentData.Select(d => d.AverageSentimentScore).ToList();

            var ratingsCount = reviews
                .GroupBy(r => r.Rating)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            var ratingLabels = Enumerable.Range(1, 5).Select(i => i.ToString()).ToList();
            var ratingData = ratingsCount;

            Console.WriteLine(ratingLabels);

            ViewData["SentimentChartData"] = new { labels = sentimentLabels, data = sentimentScores };
            ViewData["RatingChartData"] = new { labels = ratingLabels, data = ratingData };
        }
    }
}
