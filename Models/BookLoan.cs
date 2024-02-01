using System;

namespace Moment3_2.Models {
    public class BookLoan {
        public int BookLoanId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
    }
}
