using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Moment3_2.Data;
using Moment3_2.Models; 
using Moment3_2.ViewModels; 
using System.Threading.Tasks;
using System.Diagnostics;


public class HomeController : Controller
{
    private readonly BookContext _context; 

    public HomeController(BookContext context) 
    {
        _context = context; 
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            Books = await _context.Book.ToListAsync(),
            Borrowers = await _context.Borrower.ToListAsync(),
            BookLoans = await _context.BookLoan.Include(b => b.Book).Include(b => b.Borrower).ToListAsync()
        };

        return View(viewModel);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
