using System;
using System.Collections.Generic;

namespace Moment3_2.Models {
    public class Borrower {
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


        // Navigationsegenskap för att hålla reda på lånade böcker
        public List<BookLoan> BookLoans { get; set; }
    }
}
