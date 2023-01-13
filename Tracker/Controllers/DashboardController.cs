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


            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i=>i.Category)
                .OrderByDescending(j=>j.Date)
                .Take(5)
                .ToListAsync();



            //Income and Expense
            //Income
            List<SplineChart> incomeSummary = SelectedTransactions
                .Where(i=>i.Category.Type == "Income")
                .GroupBy(j=>j.Date)
                .Select(k=> new SplineChart()
                {
                    day = k.First().Date.ToString("dd/MM"),
                    income = k.Sum(l=>l.Amount)
                })
                .ToList();
            //Expense
            List<SplineChart> expenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChart()
                {
                    day = k.First().Date.ToString("dd/MM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();


            string[] days = Enumerable.Range(0,7)
                .Select(i => StartDate.AddDays(i).ToString("dd/MM"))
                .ToArray();

            ViewBag.SplineChart = from day in days
                                      join income in incomeSummary on day equals income.day into dayIncomeJoin
                                      from income in dayIncomeJoin.DefaultIfEmpty()
                                      join expense in expenseSummary on day equals expense.day into expenseJoin
                                      from expense in expenseJoin.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            return View();
        }
    }
    public class SplineChart
    {
        public string day;
        public int income;
        public int expense;
    }
}
