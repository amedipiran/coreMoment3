using Moment3_2.Models;
using Microsoft.EntityFrameworkCore;


namespace Moment3_2.Data {
public class BookContext : DbContext {
    public BookContext(DbContextOptions<BookContext> options) : base(options) {
        
    }
    public DbSet<Book> Book {get; set;}
}



}