using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class EFBookRepository : IBookRepository
    {
        private ApplicationDbContext context;

        public EFBookRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Book> Books => context.Books;


        public void SaveBook(Book book)
        {
            if (book.Id == 0)            
                context.Books.Add(book);           
            else
            {
                Book bookEntry = context.Books
                    .FirstOrDefault(r => r.Id == book.Id);

                if (bookEntry != null)                
                    bookEntry.UpdateFrom(book);                                    
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookID)
        {
            Book bookEntry = context.Books
                .FirstOrDefault(r => r.Id == bookID);

            if (bookEntry != null)
            {
                context.Books.Remove(bookEntry);
                context.SaveChanges();
            }
            return bookEntry;
        }
    }
}
