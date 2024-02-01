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
    var availableBooks = _context.Book.Where(b => b.Quantity > 0).ToList();
    var borrowers = _context.Borrower.ToList();

    if (!availableBooks.Any())
    {
        TempData["ErrorMessage"] = "This book is unavailable, try again later.";
    }

    ViewData["Books"] = new SelectList(availableBooks, "Id", "Title");
    ViewData["Borrowers"] = new SelectList(borrowers, "BorrowerId", "Name");

    return View();
}


    // POST: BookLoan/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BookLoanId,BookId,BorrowerId,LoanDate,ReturnDate")] BookLoan bookLoan)
    {
        var book = await _context.Book.FindAsync(bookLoan.BookId);
        if (book != null && book.Quantity > 0)
        {
            if (ModelState.IsValid)
            {
                book.Quantity--; // Minska antalet tillgängliga exemplar
                _context.Update(book);

                _context.Add(bookLoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        else
        {
            // Hantera fallet när boken inte är tillgänglig
        }

        // Återladda SelectList vid valideringsfel
        ViewData["Books"] = new SelectList(_context.Book, "Id", "Title", bookLoan.BookId);
        ViewData["Borrowers"] = new SelectList(_context.Borrower, "BorrowerId", "Name", bookLoan.BorrowerId);
        return View(bookLoan);
    }


        // GET: BookLoan/Edit/5
     public async Task<IActionResult> Edit(int? id)
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

    ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title", bookLoan.BookId);
    ViewData["BorrowerId"] = new SelectList(_context.Borrower, "BorrowerId", "Name", bookLoan.BorrowerId);

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

    ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title", bookLoan.BookId);
    ViewData["BorrowerId"] = new SelectList(_context.Borrower, "BorrowerId", "Name", bookLoan.BorrowerId);
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
        var bookLoan = await _context.BookLoan
            .Include(bl => bl.Book)
            .FirstOrDefaultAsync(bl => bl.BookLoanId == id);

        if (bookLoan != null && bookLoan.Book != null)
        {
            bookLoan.Book.Quantity++; // Öka antalet tillgängliga exemplar
            _context.Update(bookLoan.Book);

            _context.BookLoan.Remove(bookLoan);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


        private bool BookLoanExists(int id)
        {
            return _context.BookLoan.Any(e => e.BookLoanId == id);
        }
    }
}
