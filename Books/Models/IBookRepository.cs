using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public interface IBookRepository
    {        
        IQueryable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookID);
    }
}
