using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class BookInfoListViewModel
    {
        public Book book { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public BookImage bookImage { get; set; }
        public IEnumerable<BookImage> MyBookImages { get; set; }

        public BookSearch bookSearch { get; set; }

        public IEnumerable<Book> MyBooks { get; set; }
    }
}
