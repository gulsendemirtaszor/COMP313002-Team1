using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Models;

namespace Books.Models.ViewModels
{
    public class CategoriesListViewModel
    {
        public Category category { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}
