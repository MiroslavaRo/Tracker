using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracker.Models;

namespace Tracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly TrackerDbContext _context;

        public TransactionsController(TrackerDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var trackerDbContext = _context.Transactions.Include(t => t.Category);
            return View(await trackerDbContext.ToListAsync());
        }

        // GET: Transactions/CreateOrEdit
        public IActionResult CreateOrEdit(int id = 0)
        {
            var CategoryCollection = _context.Categories.ToList();
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            CategoryCollection.Insert(0, DefaultCategory);
            ViewData["CategoryId"] = new SelectList(CategoryCollection, "CategoryId", "TitleWithIcon");
            
            
            if (id == 0)
             {
                //create new 
                return View(new Transaction());
            }
             else
             {
                 //enter existing 
                 return View(_context.Transactions.Find(id));
             }
        }

        // POST: Transactions/CreateOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("TransactionId,Amount,Note,Date,CategoryId")] Transaction transaction)
        {
            
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                {
                    _context.Add(transaction);
                }
                else
                {
                    _context.Update(transaction);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "TitleWithIcon");
            return View(transaction);
        }


        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'TrackerDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





    }
}
