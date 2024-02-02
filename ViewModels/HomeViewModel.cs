
using Moment3_2.Models;
namespace Moment3_2.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Borrower> Borrowers { get; set; }
        public IEnumerable<BookLoan> BookLoans { get; set; }
    }
}
