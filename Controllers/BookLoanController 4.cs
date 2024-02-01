using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moment3_2.Data;
using Moment3_2.Models;

namespace Moment3_2.Controllers
{
    public class BookLoanController : Controller
    {
        private readonly BookContext _context;

        public BookLoanController(BookContext context)
        {
            _context = context;
        }

        // GET: BookLoan
        public async Task<IActionResult> Index()
        {
            var bookContext = _context.BookLoan.Include(b => b.Book).Include(b => b.Borrower);
            return View(await bookContext.ToListAsync());
        }

        // GET: BookLoan/Details/5
     public IActionResult Create()
{
    var books = _context.Book.ToList();
    var borrowers = _context.Borrower.ToList();

    if (books == null || borrowers == null)
    {
        // Hantera fallet där en eller båda listorna är null
        // Du kan antingen sätta en felmeddelande eller redirect till en annan vy
        // Till exempel:
         TempData["Error"] = "Data missing for books or borrowers.";
         return RedirectToAction("ErrorView");
    }

    ViewData["Books"] = new SelectList(books, "Id", "Title");
    ViewData["Borrowers"] = new SelectList(borrowers, "BorrowerId", "Name");
    return View();
}


        // POST: BookLoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
// POST: BookLoan/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("BookLoanId,BookId,BorrowerId,LoanDate,ReturnDate")] BookLoan bookLoan)
{
    if (ModelState.IsValid)
    {
        _context.Add(bookLoan);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Uppdatera SelectList för att behålla valda värden vid valideringsfel
ViewData["Books"] = new SelectList(_context.Book, "Id", "Title");
ViewData["Borrowers"] = new SelectList(_context.Borrower, "BorrowerId", "Name");

    return View(bookLoan);
}


        // GET: BookLoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoan.FindAsync(id);
            if (bookLoan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", bookLoan.BookId);
            ViewData["BorrowerId"] = new SelectList(_context.Borrower, "BorrowerId", "BorrowerId", bookLoan.BorrowerId);
            return View(bookLoan);
        }

        // POST: BookLoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookLoanId,BookId,BorrowerId,LoanDate,ReturnDate")] BookLoan bookLoan)
        {
            if (id != bookLoan.BookLoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookLoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLoanExists(bookLoan.BookLoanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Id", bookLoan.BookId);
            ViewData["BorrowerId"] = new SelectList(_context.Borrower, "BorrowerId", "BorrowerId", bookLoan.BorrowerId);
            return View(bookLoan);
        }

        // GET: BookLoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoan
                .Include(b => b.Book)
                .Include(b => b.Borrower)
                .FirstOrDefaultAsync(m => m.BookLoanId == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

        // POST: BookLoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookLoan = await _context.BookLoan.FindAsync(id);
            if (bookLoan != null)
            {
                _context.BookLoan.Remove(bookLoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookLoanExists(int id)
        {
            return _context.BookLoan.Any(e => e.BookLoanId == id);
        }
    }
}
