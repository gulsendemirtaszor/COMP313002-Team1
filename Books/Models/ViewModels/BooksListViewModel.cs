using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Models;

namespace Books.Models.ViewModels
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public CategoriesListViewModel CategoryListViewModel { get; set; }

        public IEnumerable<BookImage> MyBookImages { get; set; }
    }
}
