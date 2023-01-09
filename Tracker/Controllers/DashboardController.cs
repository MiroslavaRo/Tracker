using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tracker.Models;

namespace Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TrackerDbContext _context;
        public DashboardController(TrackerDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {

            //lat 7 days
            DateTime StartDate = DateTime.Today.AddDays(-60);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(a=>a.Category)
                .Where(b=>b.Date>=StartDate && b.Date<=EndDate)
                .ToListAsync();


            //Total Income
            int totalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);

            ViewBag.TotalIncome = totalIncome.ToString("C0");

            //Total Expense
            int totalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);

            ViewBag.TotalExpense = totalExpense.ToString("C0");

            //Balance 
            int Balance = totalIncome - totalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}",Balance);


            //Doughnut cahert 
              ViewBag.DoughnutCahrtData = SelectedTransactions
                   .Where(i => i.Category.Type == "Expense")
                   .GroupBy(j => j.Category.CategoryId)
                   .Select(k => new
                   {
                       categoryTitleWithIcon = k.First().Category.Icon+ k.First().Category.Title,
                       amount = k.Sum(j => j.Amount),
                       formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                   })
                   .ToList();


            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i=>i.Category)
                .OrderByDescending(j=>j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
